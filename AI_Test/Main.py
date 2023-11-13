# -*- coding:utf-8 -*-

import AI_Common
from Common import Common as cm

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
    if os.path.isfile('one_letter_100000.keras'):
        model = keras.models.load_model('one_letter_100000.keras')
    else:
        model = common.CreateModel()

    test_loss = None
    test_acc = None
    # 학습
    for i in range(0, 532658, 1000):
        # 데이터 가져오기
        x_train, y_train = common.GetTrainData(i, i+1000,'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter\\')
        if len(x_train) > 0 :
            model.fit(x_train, y_train, epochs=5, validation_data=(x_train, y_train))
            print(i+1000)
            if i+1000 % 10000 == 0:
                model.save('one_letter_'+str(i+1000)+'.keras')
                test_loss, test_acc = model.evaluate(x_train, y_train)
                print(f'Test accuracy: {test_acc}')
    
    # 모델 저장
    model.save('my_model.keras')

    # # 임시 테스트
    # textBoxs = cm.ImgToTextBox('test.png')
    # # for item in textBoxs:
    # #     cv2.imshow("item", item)
    # #     cv2.waitKey()
    # # cv2.destroyAllWindows()
    
    # model = keras.models.load_model('my_model_2310000.keras')

    # test = common.GetImgToData(textBoxs[0], 28, 28)
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