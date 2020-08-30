# EyeX ApiHost Application
Hardware-binding module of the EyeX Framework
## Features
### API Connection Module
- Establishes connection to different Eye-Tracking APIs
- Supported APIs: Tobii.Interaction, Tobii.Research (Pro)
### Data Processing
- Optional filters the raw GazePoint data with an average mean algorithm
- Implements an I-DT fixation detection algorithm based on Salvucci and Goldberg [1]
### API
Provides an interface for Client applications to
- initialize the API
- (un)subscribe to GazePoint Data
- (un)subscribe to Fixation Data


[1] Dario D. Salvucci and Joseph H. Goldberg. 2000. Identifying fixations and saccades in eye-tracking protocols. In Proceedings of the 2000 symposium on Eye tracking research & applications (ETRA '00). Association for Computing Machinery, New York, NY, USA, 71–78. DOI:https://doi.org/10.1145/355017.355028