using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Cloudspace
{

    public class CharacterHandler : MonoBehaviour
    {

        Animator anim;

        public float lookIKWeight;
        public float bodyWeight;
        public float headWeight;
        public float eyesWeight;
        public float clampWeight;

        public bool actionInProgress;

        public bool leanBack;
        public bool leanForward;
        public bool sitStraight;

        public bool lookLeft;
        public bool lookRight;
        public bool lookUp;
        public bool lookDown;
        public bool lookUpLeft;
        public bool lookUpRight;
        public bool lookDownLeft;
        public bool lookDownRight;
        public bool lookStraight;
        public bool headBack;
        public bool headForward;

        public bool eyesLeft;
        public bool eyesRight;
        public bool eyesUp;
        public bool eyesDown;
        public bool eyesUpLeft;
        public bool eyesUpRight;
        public bool eyesDownLeft;
        public bool eyesDownRight;
        public bool eyesStraight;
        public bool eyesAtMouth;
        public bool eyesOverLeft;
        public bool eyesOverRight;

        public bool nodHead;
        public bool shakeHead;

        public Transform lookPos;
        public Transform eyePos;

        private int actionCount;
        private string bodyAction;
        private string headAction;
        private bool userTalking;
        private bool userDoneTalking;
        private bool idle;
        private bool botTalking;
        private bool eyesMoved;

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            actionCount = 0;
            bodyAction = "";
            headAction = "";
            actionInProgress = false;
            eyesMoved = false;

            Cloudspace.NotificationCenter.DefaultCenter().AddObserver(this, "OnListeningToUser");
            Cloudspace.NotificationCenter.DefaultCenter().AddObserver(this, "OnStopListening");
            Cloudspace.NotificationCenter.DefaultCenter().AddObserver(this, "OnStopUserSpeech");
        }

        // When listening is happening.
        public void OnListeningToUser()
        {
            this.RunUserTalkingActions();
            this.RunActions();
        }

        public void OnStopListening()
        {
            this.RunBotTalkingActions();
            this.RunActions();
        }

        public void OnStopUserSpeech()
        {
            this.RunUserDoneActions();
            this.RunActions();
        }

        // Update is called once per frame
        void Update()
        {
            if (actionInProgress == true)
            {
                if (lookLeft == true)
                {
                    eyesMoved = true;
                    CheckLookLeft();
                }
                else if (lookRight == true)
                {
                    eyesMoved = true;
                    CheckLookRight();
                }
                else if (lookUp == true)
                {
                    eyesMoved = true;
                    CheckLookUp();
                }
                else if (lookDown == true)
                {
                    eyesMoved = true;
                    CheckLookDown();
                }
                else if (lookUpLeft == true)
                {
                    eyesMoved = true;
                    CheckLookUpLeft();
                }
                else if (lookUpRight == true)
                {
                    eyesMoved = true;
                    CheckLookUpRight();
                }
                else if (lookDownLeft == true)
                {
                    eyesMoved = true;
                    CheckLookDownLeft();
                }
                else if (lookDownRight == true)
                {
                    eyesMoved = true;
                    CheckLookDownRight();
                }
                else if (lookStraight == true)
                {
                    CheckLookStraight();
                }
                else if (headBack == true)
                {
                    CheckHeadBack();
                }
                else if (headForward == true)
                {
                    CheckHeadForward();
                }
                else if (leanBack == true)
                {
                    CheckLeanBack();
                }
                else if (leanForward == true)
                {
                    CheckLeanForward();
                }
                else if (sitStraight == true)
                {
                    CheckSitStraight();
                }
                else if (nodHead == true)
                {
                    NodHead();
                }
                else if (shakeHead == true)
                {
                    ShakeHead();
                }
                else if (eyesLeft == true)
                {
                    eyesMoved = true;
                    CheckEyesLeft();
                }
                else if (eyesRight == true)
                {
                    eyesMoved = true;
                    CheckEyesRight();
                }
                else if (eyesUp == true)
                {
                    eyesMoved = true;
                    CheckEyesUp();
                }
                else if (eyesDown == true)
                {
                    eyesMoved = true;
                    CheckEyesDown();
                }
                else if (eyesUpLeft == true)
                {
                    eyesMoved = true;
                    CheckEyesUpLeft();
                }
                else if (eyesUpRight == true)
                {
                    eyesMoved = true;
                    CheckEyesUpRight();
                }
                else if (eyesDownLeft == true)
                {
                    eyesMoved = true;
                    CheckEyesDownLeft();
                }
                else if (eyesDownRight == true)
                {
                    eyesMoved = true;
                    CheckEyesDownRight();
                }
                else if (eyesStraight == true)
                {
                    CheckEyesStraight();
                }
                else if (eyesAtMouth == true)
                {
                    eyesMoved = true;
                    CheckEyesMouth();
                }
                else if (eyesOverLeft == true)
                {
                    eyesMoved = true;
                    CheckEyesOverLeft();
                }
                else if (eyesOverRight == true)
                {
                    eyesMoved = true;
                    CheckEyesOverRight();
                }
            }
        }

        void OnAnimatorIK()
        {
            anim.SetLookAtWeight(lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
            anim.SetLookAtPosition(lookPos.position);
        }

        public void RunActions()
        {
            CancelInvoke("RunActions");
            ReturnEyesToIdle();
            if (userDoneTalking == true)
            {
                bodyAction = "sitStraight";
            }
            else
            {
                bodyAction = PickBodyAction();
            }
            if (bodyAction == "sitStraight")
            {
                headAction = PickHeadAction();
                if (eyesMoved == true)
                {
                    eyesMoved = false;
                    headAction = "eyesStraight";
                }
                else
                {
                    if (userDoneTalking == true)
                    {
                        if (headAction == "eyesOverRight" || headAction == "eyesDownRight" || headAction == "eyesDownLeft" || headAction == "eyesUpRight" || headAction == "eyesUpLeft")
                        {
                            eyesStraight = false;
                            Invoke("ReturnEyesToIdle", 1);
                        }
                        else if (headAction == "eyesAtMouth")
                        {
                            eyesStraight = false;
                            Invoke("ReturnEyesToIdle", 2);
                        }
                    }
                }
            }
            else
            {
                headAction = "";
            }
            if (headAction != "")
            {
                System.Reflection.FieldInfo FI = this.GetType().GetField(headAction);
                FI.SetValue(this, true);
            }
            else
            {
                System.Reflection.FieldInfo FI = this.GetType().GetField(bodyAction);
                FI.SetValue(this, true);
            }
            actionInProgress = true;
            if (userTalking == false)
            {
                RunIdleActions();
                Invoke("RunActions", 4);
            }
        }

        public void ReturnEyesToIdle()
        {
            eyesStraight = true;
            eyePos.transform.position = new Vector3(-1.3f, 0.6f, -0.7f);
        }

        void LookTowards(Vector3 lookVector, int speedMulti)
        {
            float step = speedMulti * Time.deltaTime;
            lookPos.transform.position = Vector3.MoveTowards(lookPos.transform.position, lookVector, step);
        }

        void EyesTowards(Vector3 eyeVector)
        {
            float step = 50 * Time.deltaTime;
            eyePos.transform.position = Vector3.MoveTowards(eyePos.transform.position, eyeVector, step);
        }

        public void CheckLookLeft()
        {
            if (lookPos.transform.position == LookLeftVec())
            {
                lookLeft = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookLeftVec(), 20);
            }
        }

        public void CheckLookRight()
        {
            if (lookPos.transform.position == LookRightVec())
            {
                lookRight = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookRightVec(), 20);
            }
        }

        public void CheckLookUp()
        {
            if (lookPos.transform.position == LookUpVec())
            {
                lookUp = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookUpVec(), 20);
            }
        }

        public void CheckLookDown()
        {
            if (lookPos.transform.position == LookDownVec())
            {
                lookDown = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookDownVec(), 20);
            }
        }

        public void CheckLookUpLeft()
        {
            if (lookPos.transform.position == LookUpLeftVec())
            {
                lookUpLeft = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookUpLeftVec(), 20);
            }
        }

        public void CheckLookUpRight()
        {
            if (lookPos.transform.position == LookUpRightVec())
            {
                lookUpRight = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookUpRightVec(), 20);
            }
        }

        public void CheckLookDownLeft()
        {
            if (lookPos.transform.position == LookDownLeftVec())
            {
                lookDownLeft = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookDownLeftVec(), 20);
            }
        }

        public void CheckLookDownRight()
        {
            if (lookPos.transform.position == LookDownRightVec())
            {
                lookDownRight = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookDownRightVec(), 20);
            }
        }

        public void CheckLookStraight()
        {
            if (lookPos.transform.position == LookStraightVec())
            {
                lookStraight = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(LookStraightVec(), 20);
            }
        }

        public void CheckHeadBack()
        {
            if (lookPos.transform.position == HeadBackVec())
            {
                headBack = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(HeadBackVec(), 20);
            }
        }

        public void CheckHeadForward()
        {
            if (lookPos.transform.position == HeadForwardVec())
            {
                headForward = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                DecreaseBodyWeight();
                LookTowards(HeadForwardVec(), 20);
            }
        }

        public void CheckLeanBack()
        {
            if (lookPos.transform.position == LeanBackVec())
            {
                leanBack = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                IncreaseBodyWeight();
                LookTowards(LeanBackVec(), 3);
            }
        }

        public void CheckLeanForward()
        {
            if (lookPos.transform.position == LeanForwardVec())
            {
                leanForward = false;
                actionInProgress = false;
            }
            else
            {
                DecreaseHeadWeight();
                IncreaseBodyWeight();
                LookTowards(LeanForwardVec(), 2);
            }
        }

        public void CheckSitStraight()
        {
            if (lookPos.transform.position == LookStraightVec())
            {
                sitStraight = false;
                actionInProgress = false;
            }
            else
            {
                IncreaseHeadWeight();
                IncreaseBodyWeight();
                LookTowards(LookStraightVec(), 1);
            }
        }

        public void CheckEyesLeft()
        {
            if (eyePos.transform.position == LookLeftVec())
            {
                eyesLeft = false;
            }
            else
            {
                EyesTowards(LookLeftVec());
            }
        }

        public void CheckEyesRight()
        {
            if (eyePos.transform.position == LookRightVec())
            {
                eyesRight = false;
            }
            else
            {
                EyesTowards(LookRightVec());
            }
        }

        public void CheckEyesUp()
        {
            if (eyePos.transform.position == LookUpVec())
            {
                eyesUp = false;
            }
            else
            {
                EyesTowards(LookUpVec());
            }
        }

        public void CheckEyesDown()
        {
            if (eyePos.transform.position == LookDownVec())
            {
                eyesDown = false;
            }
            else
            {
                EyesTowards(LookDownVec());
            }
        }

        public void CheckEyesUpLeft()
        {
            if (eyePos.transform.position == LookUpLeftVec())
            {
                eyesUpLeft = false;
            }
            else
            {
                EyesTowards(LookUpLeftVec());
            }
        }

        public void CheckEyesUpRight()
        {
            if (eyePos.transform.position == LookUpRightVec())
            {
                eyesUpRight = false;
            }
            else
            {
                EyesTowards(LookUpRightVec());
            }
        }

        public void CheckEyesDownLeft()
        {
            if (eyePos.transform.position == LookDownLeftVec())
            {
                eyesDownLeft = false;
            }
            else
            {
                EyesTowards(LookDownLeftVec());
            }
        }

        public void CheckEyesDownRight()
        {
            if (eyePos.transform.position == LookDownRightVec())
            {
                eyesDownRight = false;
            }
            else
            {
                EyesTowards(LookDownRightVec());
            }
        }

        public void CheckEyesStraight()
        {
            if (eyePos.transform.position == EyesStraightVec())
            {
                eyesStraight = false;
            }
            else
            {
                EyesTowards(EyesStraightVec());
            }
        }

        public void CheckEyesMouth()
        {
            if (eyePos.transform.position == EyesMouthVec())
            {
                eyesAtMouth = false;
            }
            else
            {
                EyesTowards(EyesMouthVec());
            }
        }

        public void CheckEyesOverLeft()
        {
            if (eyePos.transform.position == EyesOverLeftVec())
            {
                eyesOverLeft = false;
            }
            else
            {
                EyesTowards(EyesOverLeftVec());
            }
        }

        public void CheckEyesOverRight()
        {
            if (eyePos.transform.position == EyesOverRightVec())
            {
                eyesOverRight = false;
            }
            else
            {
                EyesTowards(EyesOverRightVec());
            }
        }

        public void NodHead()
        {
            if (actionCount == 0)
            {
                if (lookPos.transform.position == NodVec())
                {
                    actionCount = 1;
                    NodHead();
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(NodVec(), 3);
                }
            }
            else if (actionCount == 2)
            {
                if (lookPos.transform.position == NodVec())
                {
                    actionCount = 3;
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(NodVec(), 3);
                }
            }
            else if (actionCount == 3)
            {
                if (lookPos.transform.position == LookStraightVec())
                {
                    actionCount = 0;
                    nodHead = false;
                    actionInProgress = false;
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(LookStraightVec(), 3);
                }
            }
            else
            {
                if (lookPos.transform.position == LookStraightVec())
                {
                    actionCount = 2;
                    NodHead();
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(LookStraightVec(), 3);
                }
            }
            if (eyePos.transform.position == EyesStraightVec())
            {
                eyesStraight = false;
            }
            else
            {
                EyesTowards(EyesStraightVec());
            }
        }

        public void ShakeHead()
        {
            if (actionCount == 0)
            {
                if (lookPos.transform.position == ShakeLeftVec())
                {
                    actionCount = 1;
                    ShakeHead();
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(ShakeLeftVec(), 10);
                }
            }
            else if (actionCount == 2)
            {
                if (lookPos.transform.position == ShakeLeftVec())
                {
                    actionCount = 3;
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(ShakeLeftVec(), 10);
                }
            }
            else if (actionCount == 3)
            {
                if (lookPos.transform.position == LookStraightVec())
                {
                    actionCount = 0;
                    shakeHead = false;
                    actionInProgress = false;
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(LookStraightVec(), 10);
                }
            }
            else
            {
                if (lookPos.transform.position == ShakeRightVec())
                {
                    actionCount = 2;
                    ShakeHead();
                }
                else
                {
                    DecreaseBodyWeight();
                    IncreaseHeadWeight();
                    LookTowards(ShakeRightVec(), 10);
                }
            }
            if (eyePos.transform.position == EyesStraightVec())
            {
                eyesStraight = false;
            }
            else
            {
                EyesTowards(EyesStraightVec());
            }
        }

        public Vector3 LookLeftVec()
        {
            return new Vector3(-1.3f, 1.6f, -3.0f);
        }

        public Vector3 LookRightVec()
        {
            return new Vector3(-4.0f, 1.6f, -0.7f);
        }

        public Vector3 LookUpVec()
        {
            return new Vector3(-1.3f, 4.0f, -0.7f);
        }

        public Vector3 LookDownVec()
        {
            return new Vector3(-1.3f, 0.0f, -0.7f);
        }

        public Vector3 LookUpLeftVec()
        {
            return new Vector3(1.3f, 4.0f, -3.0f);
        }

        public Vector3 LookUpRightVec()
        {
            return new Vector3(-4.0f, 4.0f, -0.7f);
        }

        public Vector3 LookDownLeftVec()
        {
            return new Vector3(1.3f, 0.0f, -3.0f);
        }

        public Vector3 LookDownRightVec()
        {
            return new Vector3(-4.0f, 0.0f, -0.7f);
        }

        public Vector3 LookStraightVec()
        {
            return new Vector3(-1.3f, 1.3f, -0.7f);
        }

        public Vector3 HeadBackVec()
        {
            return new Vector3(-1.3f, 3.0f, -0.7f);
        }

        public Vector3 HeadForwardVec()
        {
            return new Vector3(-1.3f, 1.6f, -0.7f);
        }

        public Vector3 NodVec()
        {
            return new Vector3(-1.3f, 0.5f, -0.7f);
        }

        public Vector3 ShakeLeftVec()
        {
            return new Vector3(-1.3f, 1.3f, -1.5f);
        }

        public Vector3 ShakeRightVec()
        {
            return new Vector3(-2.5f, 1.3f, -0.7f);
        }

        public Vector3 LeanBackVec()
        {
            return new Vector3(-1.3f, 2.0f, -0.7f);
        }

        public Vector3 LeanForwardVec()
        {
            return new Vector3(-1.3f, 1.0f, -0.7f);
        }

        public Vector3 EyesStraightVec()
        {
            return new Vector3(-1.3f, 0.6f, -0.7f);
        }

        public Vector3 EyesMouthVec()
        {
            return new Vector3(-1.3f, -1.0f, -0.7f);
        }

        public Vector3 EyesOverLeftVec()
        {
            return new Vector3(-1.3f, 0.6f, -2.0f);
        }

        public Vector3 EyesOverRightVec()
        {
            return new Vector3(-3.0f, 0.6f, -0.7f);
        }

        public void IncreaseBodyWeight()
        {
            if (bodyWeight < 1f)
            {
                bodyWeight += 0.1f;
            }
        }

        public void DecreaseBodyWeight()
        {
            if (bodyWeight > 0f)
            {
                bodyWeight -= 0.1f;
            }
        }

        public void IncreaseHeadWeight()
        {
            if (headWeight < 1.0f)
            {
                headWeight += 0.1f;
            }
        }

        public void DecreaseHeadWeight()
        {
            if (headWeight > 0.0f)
            {
                headWeight -= 0.1f;
            }
        }

        private string PickBodyAction()
        {
            string[] bodyArray = new string[] {
            "leanBack",
            "leanForward",
            "leanForward",
            "sitStraight",
            "sitStraight",
            "sitStraight",
            "sitStraight",
            "sitStraight",
            "sitStraight",
            "sitStraight"
        };
            int r = UnityEngine.Random.Range(0, bodyArray.Length);
            return bodyArray[r];
        }

        private string PickHeadAction()
        {
            string[] headArray;
            if (userTalking == true)
            {
                headArray = (string[])userTalkingHeadArray();
            }
            else if (userDoneTalking == true)
            {
                headArray = (string[])userDoneTalkingHeadArray();
            }
            else if (botTalking == true)
            {
                headArray = (string[])botTalkingHeadArray();
            }
            else
            {
                headArray = (string[])idleHeadArray();
            }
            int rh = UnityEngine.Random.Range(0, headArray.Length);
            return headArray[rh];
        }

        public Array userTalkingHeadArray()
        {
            string[] headArray = new string[] {
            "eyesAtMouth"
        };
            return headArray;
        }

        public Array userDoneTalkingHeadArray()
        {
            string[] headArray = new string[] {
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight",
            "eyesUpRight"
        };
            return headArray;
        }

        public Array botTalkingHeadArray()
        {
            string[] headArray = new string[] {
            "eyesOverRight",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };
            return headArray;
        }

        public Array idleHeadArray()
        {
            string[] headArray = new string[] {
            "eyesOverRight",
            "nodHead",
            "nodHead",
            "nodHead",
            "",
            "",
            "",
            "",
            "",
            ""
        };
            return headArray;
        }

        public void RunUserTalkingActions()
        {
            userTalking = true;
            userDoneTalking = false;
            idle = false;
            botTalking = false;
        }

        public void RunUserDoneActions()
        {
            userTalking = false;
            userDoneTalking = true;
            idle = false;
            botTalking = false;
        }

        public void RunBotTalkingActions()
        {
            userTalking = false;
            userDoneTalking = false;
            idle = false;
            botTalking = true;
        }

        public void RunIdleActions()
        {
            userTalking = false;
            userDoneTalking = false;
            idle = true;
            botTalking = false;
        }
    }
}
