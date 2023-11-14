# -*- coding:utf-8 -*-

import codecs
import shutil
import json
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

if __name__ == "__main__":
    
    # common = AI_Common.Common('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Label\\printed_data_info.json')

    jsonData = None
    imageFolder = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\'
    jsonData = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\Label\\printed_data_info.json'
    bytesEncoded = None
    byteArray = None
    folderName = None
    with open(jsonData, 'r', encoding='UTF8') as f:
        jsonData = json.load(f)
        for item in jsonData['annotations']:
            if item['attributes']['type'] == '글자(음절)':
                print(item['id'])
                
                # keras image_dataset_from_directory 여기서 한글 인식 못함
                bytesEncoded = item['text'].encode(encoding='utf-8')
                byteArray = list(bytesEncoded)
                folderName = ','.join(str(e) for e in byteArray)
                
                # 폴더 있으면 옴기기만 없으면 생성수 옴기기
                if os.path.isdir(imageFolder+folderName):
                    shutil.move(imageFolder+item['id']+'.png', imageFolder+folderName+'\\'+item['id']+'.png')
                else:
                    os.mkdir(imageFolder+folderName)
                    shutil.move(imageFolder+item['id']+'.png', imageFolder+folderName+'\\'+item['id']+'.png')
    
    # bytes_encoded = None
    # byte_array = None
    # folderName = None
    # folderList = os.listdir('C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter')
    # for item in folderList:
    #     print(item)
    #     bytes_encoded = item.encode(encoding='utf-8')
    #     byte_array = list(bytes_encoded)
    #     folderName = ','.join(str(e) for e in byte_array)
    #     os.rename('C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\'+item,
    #               'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\'+folderName)
    
    
    # str_original = '가'

    # bytes_encoded = str_original.encode(encoding='utf-8')
    # print(type(bytes_encoded))
    # str_decoded = bytes_encoded.decode()
    # print(type(str_decoded))

    # byteArray = list(bytes_encoded)
    # folderName = ','.join(str(e) for e in byteArray)
    # print(folderName)
    # stringArray = folderName.split(',')
    # byteArray = []
    # for e in stringArray:
    #     byteArray.append(int(e))
    # print(byteArray)
    # print(bytes(byteArray))
    # print(bytes(byteArray).decode())
    
    # print('Encoded bytes =', bytes_encoded)
    # print('Decoded String =', str_decoded)
    # print('str_original equals str_decoded =', str_original == str_decoded)

    # print(type('가'.encode('utf-8')))
    # test = str('가'.encode('utf-8')).replace("\\","/")
    # print(test)
    # test = test.replace("/","\\")
    # print(test)
    # print(test.de)
    # print(str(test, 'utf-8'))
