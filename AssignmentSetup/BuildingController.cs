using AssignmentSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetup
{


    public class BuildingController
    {
        //Create interface objects:
        private IDoorManager doorManager;
        private ILightManager LightManager;
        private IFireAlarmManager fireAlarmManager;
        private IWebService webService;
        private IEmailService emailService;


        //interface constructor         //L3R1
        public BuildingController(string id, ILightManager iLightManager, IFireAlarmManager iFireAlarmManager,
                IDoorManager iDoorManager, IWebService iWebService, IEmailService iEmailService)
        {
            //construct as normal, without inputted state.
            string lowerCaseId = id.ToLower();
            buildingID = lowerCaseId;

            currentState = "out of hours";

            //assign "fake" interfaces to the values held in the instantiated objects from the real interfaces
            iLightManager = LightManager;
            iFireAlarmManager = fireAlarmManager;
            iDoorManager = doorManager;
            iWebService = webService;
            iEmailService = emailService;
        }

        //constructor L1R1   +  L1R3
        public BuildingController(string id)        //L1R1 + L1R3
        {
            string lowerCaseID = id.ToLower();
            buildingID = lowerCaseID;

            currentState = "out of hours";          //L1R5

        }

        //constructor taking 2 parameters L2R3
        public BuildingController(string id, string startState)
        {
            string IDlower = id.ToLower();
            string SSLower = startState.ToLower();

            buildingID = IDlower;

            if (SSLower == "open" || SSLower == "out of hours" || SSLower == "closed")
            {
                currentState = SSLower;
            }
            else
            {
                throw new ArgumentException("Argument Exception: BuildingController can" +
                    " only be initialised to the following states 'open', 'closed', 'out of hours'");
            }

        }

        public BuildingController(string id, IDoorManager iDoorManager)
        {
            iDoorManager = doorManager;
        }

        //private variables
        private string buildingID;
        private string currentState;
        private string stateBeforeAlarm;

        //public methods
        public string GetCurrentState()             //L1R6
        {
            return currentState;
        }

        public bool SetCurrentState(string state)   //L1R7 + L2R1 + L2R2                     REFINE CODE
        {
            string stateLowerCase = state.ToLower();

            //check to see if the state requested is valid.
            if (stateLowerCase == "closed" || stateLowerCase == "out of hours" || stateLowerCase == "open" 
                    || stateLowerCase == "fire drill" || stateLowerCase == "fire alarm")
            {
                //check to see if state change is allowed.
                if (currentState == "closed" && stateLowerCase == "out of hours")
                {
                    currentState = stateLowerCase;
                    return true;
                }
                else if (currentState == "out of hours" && stateLowerCase == "open" || stateLowerCase == "closed")
                {
                    currentState = stateLowerCase;
                    return true;
                }
                else if (currentState == "open" && stateLowerCase == "out of hours")
                {
                    doorManager.OpenAllDoors();         //L3R4
                    currentState = stateLowerCase;
                    return true;
                }
                else if (stateLowerCase == "fire drill" || stateLowerCase == "fire alarm")
                {
                    stateBeforeAlarm = currentState;
                    currentState = stateLowerCase;
                    return true;
                }
                else if (currentState == stateLowerCase)
                {
                    return true;
                    throw new ArgumentException("State has not changed as it was already in {0}", currentState);
                }
                else if (currentState == "open" && stateLowerCase == "closed" && currentState == stateLowerCase)
                {
                    return false;
                    throw new ArgumentException("cannot change from open to closed");
                }
                else if (currentState == "closed" && stateLowerCase == "open")
                {
                    return false;
                    throw new ArgumentException("cannot change from open to closed");
                }

                //alarms (need another var to store the state before these 2 states were entered so that we can correctly follow harels notation.)
                else if (currentState == "fire drill")
                {
                    currentState = stateBeforeAlarm;
                    return true;
                }
                else if (currentState == "fire alarm")
                {
                    currentState = stateBeforeAlarm;
                    return true;
                }
                else
                {
                    return false;
                    throw new ArgumentException("Changing to " + currentState + " from " + stateLowerCase + " is not allowed ");
                }

            }
            else
            {
                return false;
                throw new ArgumentException("State entered is not valid");
            }
        }

        public string GetBuildingID()               //L1R2
        {
            return buildingID;
        }

        public void SetBuildingID(string id)        //L1R4
        {
            string lowerCaseID = id.ToLower();
            buildingID = lowerCaseID;
        }

        public string GetStatusReport()     //code
        {
            return "no data";
        }

        //end of buildingController class
    }

}


//General notes

/*
 You may need to install NUnit3 Test Adapter for BCT.cs to run correctly (could just be conflicting NUget adapters on my VS setup).
 All of my tests are working. (18/03/2020)

I clearly dont understand how to implement the abstract class into the 3 interfaces, i had made the absract class,
then made each manager a child class of the parent <<absract>> manager class, with the interfaces then within said class
however this breaks the given interface implementation in this file, and as we are told not to implement the classes i dont know how to proceed.

I think if i did not have poor health currently i would be able to understand the intended implementation and be 
able to move onto L3R2.. as i do understand how to make the stubs and mocks do the desired behaviour.

I used this site to understand how to include caught exceptions that were thrown in my NUnit tests:
https://nunit.org/docs/2.5/exceptionAsserts.html
     */


//defunct code


public abstract class Manager
{
    private bool engineerRequired;

    public string GetStatus()
    {
        string status = " ";
        return status;
    }

    public bool SetEngineerRequired(bool needsEngineer)
    {
        if (needsEngineer == true)
        {
            engineerRequired = true;
            return true;
        }

        else
        {
            return false;
        }
    }

}
