import cv2
import mediapipe as mp
from flask import Flask, jsonify, Response

app = Flask(__name__)

# Initialize Mediapipe Hand model
mp_hands = mp.solutions.hands
hands = mp_hands.Hands()
mp_draw = mp.solutions.drawing_utils

# Global variable to store hand position
hand_position = None

def get_hand_position():
    global hand_position
    cap = cv2.VideoCapture(0)
    
    while cap.isOpened():
        ret, frame = cap.read()
        if not ret:
            break
        
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        result = hands.process(rgb_frame)
        
        if result.multi_hand_landmarks:
            for hand_landmarks in result.multi_hand_landmarks:
                for id, lm in enumerate(hand_landmarks.landmark):
                    h, w, c = frame.shape
                    cx, cy = int(lm.x * w), int(lm.y * h)
                    hand_position = {'id': id, 'x': cx, 'y': cy}
                    break
                break

@app.route('/track_hand', methods=['GET'])
def track_hand():
    return jsonify(hand_position)

if __name__ == '__main__':
    from threading import Thread
    thread = Thread(target=get_hand_position)
    thread.start()
    app.run(debug=True, use_reloader=False)
