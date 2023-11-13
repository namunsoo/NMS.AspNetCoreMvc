# -*- coding:utf-8 -*-

from tensorflow import keras
import numpy as np
import json
import cv2
import os

class Common:
    # 파일 정도 가져오기
    def __init__(self, path):
        # 공통 데이터용
        self.json_data = None
        self.label = []
        
        # 학습 데이터용
        self.x_train = []
        self.y_train = []
        self.img = None
        self.tempImg = None
        self.tempLabelIndex = None
        self.count = 0
        
        # 이미지 가져오기 용
        self.resized = None
        self.resized_width = None
        self.resized_height = None
        self.background = None
        self.x_offset = None
        self.y_offset = None
        
        with open(path, 'r', encoding='UTF8') as f:
            self.json_data = json.load(f)
            for item in self.json_data['annotations']:
                if item['attributes']['type'] == '글자(음절)':
                    self.label.append(item['text'])
                    
        # with open(path, 'r', encoding='UTF8') as f:
        #     self.json_data = json.load(f)
        #     for item in self.json_data['annotations']:
        #         if item(['text']) == 1:
        #             self.label.append(item['text'])

        self.label = list(set(self.label))

    # 모델 생성
    def CreateModel(self):
        # AI 모델
        model = keras.Sequential([
            keras.layers.Input(shape=(28, 28, 1)),
            keras.layers.Conv2D(64, (3, 3), activation='relu'),
            keras.layers.MaxPooling2D((2, 2)),
            keras.layers.Conv2D(128, (3, 3), activation='relu'),
            keras.layers.Flatten(),
            keras.layers.Dense(len(self.label), activation='softmax')  # Change 10 to the number of classes in your dataset
        ])
        
        # model = keras.Sequential([
        #     keras.layers.Flatten(input_shape=(56, 56)),
        #     keras.layers.Dense(len(self.label))
        # ])

        # 모델 컴파일
        # model.compile(optimizer='adam',
        #           loss=keras.losses.SparseCategoricalCrossentropy(from_logits=True),
        #           metrics=[keras.metrics.SparseCategoricalAccuracy()])

        # model.compile(optimizer='adam',
        #       loss=keras.losses.SparseCategoricalCrossentropy(from_logits=True),
        #       metrics=['accuracy'])

        model.compile(loss=keras.losses.SparseCategoricalCrossentropy(from_logits=True),
            optimizer=keras.optimizers.Adam(),
            metrics=['accuracy'])
        return model
    
    # 학습 데이터 가져오기
    def GetTrainData(self, start, end, path):
        self.x_train = []
        self.y_train = []
        self.img = None
        self.tempImg = None
        self.tempLabelIndex = None
        self.count = 0
        for item in self.json_data['annotations']:
            if int(item['id']) >= start and int(item['id']) < end:
                if os.path.isfile(path + item['image_id'] + '.png'):
                    self.img = cv2.imread(path + item['image_id'] + '.png', cv2.IMREAD_GRAYSCALE) # 이미지 가져오기 + 이미지 전처리
                    self.tempImg = self.GetImgToData(self.img, 56, 56)
                    self.tempLabelIndex = self.label.index(item['text'])
                    self.x_train.append(self.tempImg)
                    self.y_train.append(self.tempLabelIndex)
                    self.count += 1;

        self.x_train = np.asarray(self.x_train).reshape(self.count, 56, 56, 1).astype('float32') / 255.0
        self.y_train = np.asarray(self.y_train)
        
        return self.x_train, self.y_train, 

    # 이미지 가져오기
    def GetImgToData(self, image, width, height, inter = cv2.INTER_AREA):
        
        # 이미지 resize
        self.resized = cv2.resize(image, (width,height), interpolation = inter)

        # 배경색 흰색 및 여백 추가
        self.resized_width = self.resized.shape[1]
        self.resized_height = self.resized.shape[0]
        self.background = np.ones((height, width), np.uint8) * 255
        self.x_offset = (width - self.resized_width) // 2
        self.y_offset = (height - self.resized_height) // 2
        self.background[self.y_offset:self.y_offset+self.resized_height, self.x_offset:self.x_offset+self.resized_width] = self.resized
    
        return self.background
    
    # 결과 값 추출(임시)
    def GetByText(self, labelNum):
        
        return self.label[labelNum]
