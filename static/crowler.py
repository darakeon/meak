from boto3 import client
from os import path, listdir, environ
from re import search, sub
from requests import get, packages

packages.urllib3.disable_warnings() 


# pip install boto3
# LOCAL_SITE 172.24.112.1
# ACCESS_KEY
# SECRET_KEY
# BUCKET


class Crowler:

    ENCODING = 'utf-8'


    def __init__(self, local_url, public_url, access_key, secret_key, bucket):
        self.local_url = f'https://{local_url}'
        self.public_url = public_url

        self.s3 = client(
            's3', 
            aws_access_key_id=access_key,
            aws_secret_access_key=secret_key,
        )

        self.bucket = bucket


    def upload_dynamic(self):

        url = self.local_url
        file_path = f'home.html'
        self._upload_html(url, file_path)

        stories_path = path.join('..', 'stories')
        seasons_episodes = self._get_stories(stories_path)

        for season in seasons_episodes:

            url = f'{self.local_url}/{season}.meak'
            file_path = f'{season}.html'

            self._upload_html(url, file_path)

            episodes = seasons_episodes[season]

            for episode in episodes:

                url = f'{self.local_url}/{season}{episode}.meak'
                file_path = f'{season}{episode}.html'

                self._upload_html(url, file_path)

    def _get_stories(self, stories_path):
        seasons_episodes = {}

        for stories_item in listdir(stories_path):
            stories_item_path = path.join(stories_path, stories_item)

            if path.isdir(stories_item_path) and search(r'^\_[A-Z]$', stories_item):
                episodes = []

                for season_item in listdir(stories_item_path):
                    season_item_path = path.join(stories_item_path, season_item)

                    if path.isdir(season_item_path) and search(r'^\d{2}$', season_item):
                        episode = season_item
                        episodes.append(episode)

                season = stories_item[1]
                seasons_episodes[season] = episodes

        return seasons_episodes

    def _upload_html(self, url, file_path):
        content = get(url, verify=False).content.decode(self.ENCODING)

        content = self._clear_html(content)

        with open(file_path, 'w') as file:
            file.write(content)

        extra_args = {
            'ContentType': 'text/html',
            'ContentEncoding': 'utf-8',
        }

        self._upload(file_path, file_path, extra_args)

    def _clear_html(self, content: str):
        content = content.replace(self.local_url, self.public_url)

        internal_link_pattern = r"([\"'])([A-Z]\d*).meak\1"

        content = sub(internal_link_pattern, r"\1\2.html\1", content)

        return content


    def upload_static(self):
        site_path = path.join('..', 'site', 'Presentation')
        files = self._get_files(site_path, 'Assets', ['Author', 'matrix'])

        for file in files:
            extra_args = {} # 'ContentEncoding': 'utf-8'

            if file['path'].endswith('.css'):
                extra_args['ContentType'] = 'text/css'

            if file['path'].endswith('.js'):
                extra_args['ContentType'] = 'text/javascript'

            self._upload(file['path'], file['bucket'], extra_args)

    def _get_files(self, main_folder, relative_folder, ignore):
        main_path = path.join(main_folder, relative_folder)
        files = []

        for item in listdir(main_path):
            item_path = path.join(main_path, item)
            item_bucket = path.join(relative_folder, item)

            if path.isfile(item_path):
                files.append({
                    'path': item_path,
                    'bucket': item_bucket,
                })

            elif path.isdir(item_path) and item not in ignore:
                files = files + self._get_files(main_folder, item_bucket, ignore)

        return files


    def _upload(self, path, object, extra_args):
        print('Upload', path)
        self.s3.upload_file(
            path,
            self.bucket,
            object,
            ExtraArgs=extra_args
        )



crowler = Crowler(
    environ['LOCAL_SITE'],
    'meak.com.br',
    environ['ACCESS_KEY'],
    environ['SECRET_KEY'],
    environ['BUCKET'],
)

crowler.upload_dynamic()
crowler.upload_static()
