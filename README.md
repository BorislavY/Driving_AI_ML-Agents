# AI Driving with ML-Agents in Unity

## Running this project
***

1. Clone this repository and open it as a Unity Project through Unity Hub. I used Unity version **2021.3.12f1 LTS**.


2. Open the **DrivingAITraining** scene, press Play, and watch some sick AI drifts! 

    1. Switching to Display 2 on the Game window will give you a 3rd person perspective on one of the agents.

    2. To have a drive for yourself, select the first agent (the one with the camera), then under Behavior 
    Parameters change the Behavior Type to Heuristic Only. Press Play and use WASD.


## Setting up with ML-Agents [Release 19](https://github.com/Unity-Technologies/ml-agents/releases/tag/release_19)
***

1. Install [Python 3.7](https://www.python.org/downloads/release/python-379/)


2. Create a virtual environment with Python 3.7:
```console
pip install virtualenv
virtualenv venv -p python3.7
```


3. Activate the virtual environment:
```console
venv\Scripts\activate.bat
```


4. Install the relevant packages inside your virtual environment:

    1. LTS PyTorch compatible with your CUDA or the CPU version if you don’t have CUDA set up. Use 
    [this link](https://pytorch.org/get-started/locally/) to find the appropriate command for your setup.
    For me, since I have CUDA 11 set up, it is:
    ```console
    pip3 install torch==1.8.2 torchvision==0.9.2 torchaudio===0.8.2 --extra-index-url 
    ```
    2. Install the ML-Agents Python package version 0.28.0, which corresponds to Release 19 of ML-Agents:
    ```console
    pip install mlagents==0.28.0
    ```
    3. Downgrade one of the ML-Agents dependencies to not get an error:
    ```console
    pip install importlib-metadata==4.4
    ```

5. When creating a new ML-Agents Unity project install the ML-Agents Unity packages with versions corresponding to 
   Release 19 (already done for this project):
   1. Open the Package Manager, hit the "+" button, and select "Add package from git URL". In the dialog enter:
   ```console
    git+https://github.com/Unity-Technologies/ml-agents.git?path=com.unity.ml-agents#release_19
    ```
   2. Hit the "+" button and select "Add package from git URL" again. In the dialog enter:
   ```console
   git+https://github.com/Unity-Technologies/ml-agents.git?path=com.unity.ml-agents.extensions#release_19
    ```

## Using ML-Agents
***

To quickly test training inside the Unity Editor, open a command prompt, activate your virtual 
environment, navigate to the project directory, run the following command, then press play inside the Unity project:
```console
mlagents-learn Assets\Configs\DrivingAI_IL.yaml --run-id=DrivingAIRun1
```
This training setup uses reinforcement learning along with imitation learning from demonstrations I recorded to teach 
the agent how to drive the track. More specifically, [PPO](https://openai.com/blog/openai-baselines-ppo/) along with 
[GAIL](https://arxiv.org/abs/1606.03476) and Behavioral Cloning (i.e. supervised learning) is being used. 
The [DrivingAI_IL.yaml](Assets/Configs/DrivingAI_IL.yaml) config file contains the hyperparameter details.

For more details on how the project works and how ML-Agents is utilised, see **[my video](https://drive.google.com/file/d/1YveILeV5rLUFey9_JCqVJuBEGXI79Aa_/view?usp=share_link)**.

For a quick introduction to ML-Agents you can refer to [this YouTube playlist](https://www.youtube.com/watch?v=zPFU30tbyKs&list=PLzDRvYVwl53vehwiN_odYJkPBzcqFw110).

For a more detailed introduction to ML-Agents refer to [the documentation](https://github.com/Unity-Technologies/ml-agents/tree/release_19_docs/docs).


## References
***
Training setup inspiration - [video 1](https://youtu.be/2X5m_nDBvS4), [video 2](https://youtu.be/n5rY9ffqryU).

Checkpoint system - [video](https://youtu.be/IOYNg6v9sfc).

Free assets - [Car](https://assetstore.unity.com/packages/tools/physics/prometeo-car-controller-209444), 
[Racing Kit](https://www.kenney.nl/assets/racing-kit).
