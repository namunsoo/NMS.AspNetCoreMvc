# -*- coding:utf-8 -*-

import shutil
import json
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

if __name__ == "__main__":
    
    # common = AI_Common.Common('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Label\\printed_data_info.json')
    json_data = None
    label = []
    path = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\Label\\printed_data_info.json'
    with open(path, 'r', encoding='UTF8') as f:
        json_data = json.load(f)
        for item in json_data['annotations']:
            if item['attributes']['type'] == '글자(음절)':
                label.append(item['text'])
    imageFolder = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\'
    if os.path.isfile('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\01'):
        shutil.move('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\test.png', 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\01\\test.png')
    else:
        os.mkdir('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\01')
        shutil.move('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\test.png', 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\Test\\01\\test.png')