import cv2
import numpy as np

class Common:
    def ImgToTextBox(path):
        img = cv2.imread(path)

        # convert to grayscale
        gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

        # threshold
        thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY+cv2.THRESH_OTSU)[1]

        # invert
        thresh = 255 - thresh

        # apply horizontal morphology close
        kernel = np.ones((5 ,191), np.uint8)
        morph = cv2.morphologyEx(thresh, cv2.MORPH_CLOSE, kernel)

        # get external contours
        contours = cv2.findContours(morph, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
        contours = contours[0] if len(contours) == 2 else contours[1]

        # draw contours
        result = img.copy()

        textBoxs = []
        for cntr in contours:
            # get bounding boxes
            x,y,w,h = cv2.boundingRect(cntr)
            textBoxs.append(gray[y:y+h,x:x+w])

        return textBoxs