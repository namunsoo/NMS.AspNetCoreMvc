# -*- coding:utf-8 -*-

import AI_Common

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
    # OneLetter
    directory = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter'
    img_width = 28
    img_height = 28
    batch_size = 32
    train_ds, val_ds = keras.utils.image_dataset_from_directory(
        directory,
        labels='inferred',
        label_mode='categorical',
        class_names=None,
        color_mode='grayscale',
        batch_size=batch_size,
        image_size=(img_height, img_width),
        shuffle=True,
        seed=123,
        validation_split=0.2,
        subset="both",
        crop_to_aspect_ratio=True
    )
    
    # class_names = train_ds.class_names
    # print(class_names)
    
    num_classes = len(train_ds.class_names)
    # print(num_classes)
    
    model = keras.Sequential([
        keras.layers.Rescaling(1./255, input_shape=(img_height, img_width, 1)),
        keras.layers.Conv2D(64, (3, 3), activation='relu'),
        keras.layers.MaxPooling2D((2, 2)),
        keras.layers.Conv2D(128, (3, 3), activation='relu'),
        keras.layers.Flatten(),
        keras.layers.Dense(num_classes)
    ])
    
    model.compile(loss='categorical_crossentropy',
        optimizer=keras.optimizers.Adam(),
        metrics=['accuracy']
    )
    
    model.fit(
        train_ds,
        validation_data=val_ds,
        epochs=5
    )
    
    model.save('my_model.keras')
    test_loss, test_acc = model.evaluate(val_ds)
    print(f'Test accuracy: {test_acc}')