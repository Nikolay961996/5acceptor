import http.client
import json
import pprint
import sys
import time
from datetime import datetime
import requests
import os

def evraseSend(code_snippet_content, in_file):
    #with open(in_file, 'r', encoding='utf-8') as code_snippet:
     #   code_snippet_content = code_snippet.read()

    #Названия для шапки
    current_date = datetime.today().strftime('%d-%m-%Y')
    t = time.localtime()
    current_time = time.strftime("%H.%M.%S", t)
    project_name = in_file.split('.')[0]
    # Получаем время последнего изменения файла
    file_mod_time = os.path.getmtime(in_file)
    modification_time = datetime.fromtimestamp(file_mod_time)
    formatted_time = modification_time.strftime('%d.%m.%Y %H:%M:%S')
    # print(formatted_time)

    payload = {
        'model': 'mistral-nemo-instruct-2407',
        'messages': [
            {
            'role': 'system',
            'content':
                'Отвечаешь на русском.'
                'Тебе присылают файлы для код ревью.'
                ' Ты проверяешь код на потенциальные ошибки.'
                ' После этого выдаешь их перечень и по возможности предлагаешь их исправление.'
                'Не нужно предоставлять итоговый и исходный весь код .'
            },
            {
                'role': 'system',
                'content':
                    'При генерации ревью учитывай следующие ошибки:'
                    'Nuget пакеты, по возможности, должны быть обновлены до последних версий и'
                    ' не иметь уязвимостей;'
                    'если транзитивный Nuget пакет имеет уязвимость, значит он должен быть включен в проект'
                    'и обновлен до актуальной версии;'
                    'проект не должен иметь лишних зависимостей или зависимостей, ссылающихся на'
                    'локальные файлы (для таких зависимостей прописан абсолютный путь);'
                    'в коде не должно быть неразрешенных TODO'
                    'не должно быть закомментированного или неиспользуемого кода;'
                    'код с атрибутом Obsolete , по возможности, должен быть удален;'
                    'обязательное наличие комментариев для моделей, сущностей и т.д.;'
                    'стоит обращать внимание на неиспользуемые переменные или'
                    ' неиспользуемый возврат из методов'
            },
            {
                "role": "user",
                "content": "Сейчас ты являешься эксертом в облости программирования и проводишь код ревью по С#, JS, Python."
            },
            {
                "role": "user",
                "content": "Изучи ссылки основываясь на ресурсах: https://metanit.com/sharp/, https://learn.microsoft.com/ru-ru/dotnet/csharp/."
            },
            {
                "role": "user",
                "content": f"Ответ должен выглядить так: Анализ проекта {project_name} от {current_date} {current_time} UTC+3"
                f"Дата последнего изменения проекта : {formatted_time} UTC+3"
            },
            {
                "role": "user",
                "content": "Приводи куски кода с замечаниями и ниже исправленный вариант кода"
            },
            {
                'role': 'user',
                'content': 'Форматирование и пример ревью кода далее:'
                'Анализ проекта ```project_name``` от 01.01.2024 00:00:00 UTC+3 '
                '---'
                'Дата последнего изменения проекта : 01.01.2024 00:00:00 UTC+3'
                ''
                'Общее количество ошибок: 3'
                'Архитектурных нарушений: 1'
                '... _<Перечисление других классов ошибок>_'
                'Несоответствий стандартам: 1 '
                ''
                '### Архитектурное нарушение'
                '> `chat_service.py` (номер строки:номер символа, при наличии)'
                '>  Необходимо вынести в слой адаптеров, работать через репозитории и интерфейсы из сервисов'
                ''
                '```python'
                'user = User.query.filter_by(username=token).first()'
                'location = Location.query.filter_by(name=name).first()'
                '```'
                ''
                '### Краткое описание нарушения (Add braces to if statement)'
                '> `LinkFragmentValidator.cs` (номер строки:номер символа, при наличии)'
                '> `Severity`	`Code`	`Description`	`Project`	`File`	`Line`	'
                '> Error (active)	RCS1007	Add braces to if statement	Eurofurence.App.Server.Services	LinkFragmentValidator.cs    35'
                ''
                '```csharp'
                'if (!Guid.TryParse(fragment.Target, out Guid dealerId))'
                '    return ValidationResult.Error("Target must be of typ Guid");'
                '```'
                '> Предложенное исправление'
                ''
                '```csharp'
                'if (!Guid.TryParse(fragment.Target, out Guid dealerId)) {'
                '    return ValidationResult.Error("Target must be of typ Guid");'
                '}'
                '```'
                ''
                '### Некорректное наименование'
                '> `ui.tsx` (номер строки:номер символа, при наличии)'
                '> Поскольку этот тип относится к компоненту ProductItem и отражает его интерфейс, то тип должен называться ProductItemProps'
                ''
                '```ts'
                'type ProductProps = {'
                '  product: Product;'
                '  theme: Theme;'
                '  setProduct: (product: Product) => void;'
                '};'
                '```'
                ''
                '> Предложенное исправление'
                ''
                '```ts'
                'type ProductItemProps = {'
                '  product: Product;'
                '  theme: Theme;'
                '  setProduct: (product: Product) => void;'
                '};'
                '```'
            },
            {
                'role': 'user',
                'content': 'Проведи ревью следующего кода:\r\n' + code_snippet_content
            },
        ],
        'max_tokens': 1024,
        'temperature': 0.3
    }

    payload_dumped = json.dumps(payload)

    headers = {
        'Content-type': 'application/json',
        'Authorization': '8ZrRwz0pEI59XMUrPyM82EiHc3HgHPT2'
    }

    print("request prepared")
    response = requests.post('http://84.201.152.196:8020/v1/completions', payload_dumped, headers=headers).json()
    print("response taked")

    answer = ""
    if response and response.get('error', None) is not None:
        raise Exception(response)
    elif response.get('response_id', None) is not None:
        answer = response.get('choices')[0].get('message').get('content')
    
    return answer
        # with open("answer.txt", 'w+', encoding='utf-8') as out_file:
        #     out_file.write(answer)
