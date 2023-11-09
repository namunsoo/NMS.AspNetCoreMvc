# -*- coding:utf-8 -*-

import AI_Common

from tensorflow import argmax
from tensorflow import keras
import numpy as np
import cv2
# 오류 제거
# (선능을 높일수 있다 어쩌구 저쩌구)
# this tensorflow binary is optimized to use available cpu instructions in performance-critical operations.
# 이 텐서플로우 바이너리는 성능이 중요한 작업에서 사용 가능한 CPU 명령을 사용하도록 최적화되어 있습니다.
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

if __name__ == "__main__":
    
    model = None
    x_train = None
    y_train = None
    
    # 기본값 설정
    common = AI_Common.Common('C:\\Users\\namunsoo\\Downloads\\AI_Data\\Label\\printed_data_info.json')
    
    # 모델 생성
    # model = keras.models.load_model('one_letter_530000.keras')
    # print('model :', model.get_weights())
    # # model.summary()
    
    # model = keras.models.load_model('one_letter_430000.keras')
    # print('model :', model.get_weights())
    # # model.summary()
    
    # model = keras.models.load_model('one_letter_330000.keras')
    # print('model :', model.get_weights())
    # # model.summary()
    
    model = keras.models.load_model('one_letter_230000.keras')
    print('model :', model.get_weights())
    # model.summary()

    # x_train, y_train = common.GetTrainData(0, 1000,'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\')
    
    # # 학습
    # # x_train, y_train = common.GetTrainData(500000,510000,'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\')
    # model.fit(x_train, y_train, epochs=5, validation_data=(x_train, y_train))
    
    # # 마지막 학습 데이터로 테스트
    # test_loss, test_acc = model.evaluate(x_train, y_train)
    # print(f'Test accuracy: {test_acc}')
    
    # # 임시 테스트
    # testImg = cv2.imread('test.png', cv2.IMREAD_GRAYSCALE)
    
    # model = keras.models.load_model('my_model.keras')

    # test = common.GetImgToData(testImg, 28, 28)
    # print(test)
    # # Perform inference
    # #predictions = model.predict(test)  # Expand dimensions to create a batch of one image
    # predictions = model.predict(np.expand_dims(test, axis=0))  # Expand dimensions to create a batch of one image
    # print(len(predictions))
    # print(len(predictions[0]))
    # labelNum = np.where(predictions[0] == 1)[0][0]
    # print(labelNum)
    # print(common.GetByText(labelNum))
    # #print()
    # # Decode the model's output (e.g., using argmax for demonstration)
    # # recognized_text = ''.join([str(argmax(pred, axis=1).numpy()[0]) for pred in predictions])

    # # print("Recognized Text:", recognized_text)

