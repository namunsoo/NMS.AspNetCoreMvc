# -*- coding:utf-8 -*-

import numpy as np
import PIL
import tensorflow as tf

from tensorflow import keras
from tensorflow.keras import layers
from tensorflow.keras.models import Sequential

# 오류 제거
# (선능을 높일수 있다 어쩌구 저쩌구)
# this tensorflow binary is optimized to use available cpu instructions in performance-critical operations.
# 이 텐서플로우 바이너리는 성능이 중요한 작업에서 사용 가능한 CPU 명령을 사용하도록 최적화되어 있습니다.
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

if __name__ == "__main__":
    
    # OneLetter
    directory = 'C:\\Users\\namunsoo\\Downloads\\AI_Data\\OneLetter'
    img_width = 180
    img_height = 180
    batch_size = 32
    train_ds, val_ds = keras.utils.image_dataset_from_directory(
        directory,
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
    
    model = Sequential([
        layers.Rescaling(1./255, input_shape=(img_height, img_width, 1)),
        layers.Conv2D(16, 3, padding='same', activation='relu'),
        layers.MaxPooling2D(),
        layers.Conv2D(32, 3, padding='same', activation='relu'),
        layers.MaxPooling2D(),
        layers.Conv2D(64, 3, padding='same', activation='relu'),
        layers.MaxPooling2D(),
        layers.Dropout(0.2),
        layers.Flatten(),
        layers.Dense(128, activation='relu'),
        layers.Dense(num_classes)
    ])
    
    model.compile(optimizer=keras.optimizers.Adam(),
        loss=keras.losses.SparseCategoricalCrossentropy(from_logits=True),
        metrics=[keras.metrics.SparseCategoricalAccuracy()])
    
    # model.summary()

    model.fit(
        train_ds,
        validation_data=val_ds,
        epochs=15
    )
    
    model.save('my_model.keras')
    test_loss, test_acc = model.evaluate(val_ds)
    print(f'Test accuracy: {test_acc}')

    