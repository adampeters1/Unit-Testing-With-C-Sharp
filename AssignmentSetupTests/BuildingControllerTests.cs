using AssignmentSetup;
using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSetupTests
{

    [TestFixture]   // Parent Test class for the building controller (BC).
    public class BuildingControllerTests
    {

        [TestFixture]   //Child test class 1 for BC constructor Tests.
        public class ContructorTests
        {
            [Test]                          //(L1R1 + L1R2)
            public void ConstructorSetsBuildingID()             //Does the contructor set the ID to the value provided?
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("001");
                string output = BC.GetBuildingID();             //This also shows the GBID is working as well, otherwise return would fail.

                //Assert
                Assert.AreEqual("001", output);

            }

            [Test]                              //(L1R3)
            public void ConstructorSetsBuildingIDToLowerCase()  //Does C set the string to lower case?
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C AND T");
                string output = BC.GetBuildingID();

                //Assert
                Assert.AreEqual("c and t", output);

            }

            [Test]                              //(L1R5)
            public void ConstructorSetsInitialValueOfCurrentState()  //Does C set the value of Current state?
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C AND T");
                string output = BC.GetCurrentState();

                //Assert
                Assert.AreEqual("out of hours", output);

            }

            //END OF CONSTRUCTOR TESTS.
        }

        [TestFixture]   //Child test class 2 for BC SetBuildingIDTests.
        public class SetBuildingIDTests
        {
            [Test]                              //(L1R4)
            public void SetBuildingIDSetsCorrectValue()     //ensure SBID sets the inputted string correctly.
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C and t");
                BC.SetBuildingID("The C AND T building");
                string BID = BC.GetBuildingID();

                //Assert
                Assert.AreEqual(BID, "the c and t building");
            }

            //END OF SetBuildingID TESTS.
        }


        [TestFixture]   //Child test class 3 for BC GetCurrentState Tests.
        public class GetCurrentStateTests
        {
            [Test]                              //(L1R6)
            public void GetCurrentStateReturnsCurrentStateValue()   //Does GCS return the correct value of currentstate?
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C AND T");
                BC.SetCurrentState("open");               //set here to test if it is returning this rather than the default OOH state.
                string output = BC.GetCurrentState();

                //Assert
                Assert.AreEqual("open", output);

            }

            //END OF GetCurrentState TESTS.
        }

        [TestFixture]   //Child test class 4 for BC SetCurrentState Tests.
        public class SetCurrentStateValidStateTests       //(L1R7  P1)
        {
            //Test cases (all should be set successfully)
            [TestCase("closed")]
            [TestCase("out of hours")]
            [TestCase("open")]
            [TestCase("fire drill")]
            [TestCase("fire alarm")]

            public void SetCurrentStateSetsValidState(string state)     //SCS should set all 5 valid states successfully.
            {
                //Arrange
                BuildingController BC;
                string validState = state;

                //Act
                BC = new BuildingController("C and T");
                BC.SetCurrentState(validState);
                string stateSet = BC.GetCurrentState();

                //Assert
                Assert.AreEqual(stateSet, validState);

            }

            [Test]                               //(L1R7  P2)
            public void SetCurrentStateRejectsInvalidState()            //SCS should reject the invalid string
            {
                //Arrange
                BuildingController BC;
                string invalidState = "Building is under maintainance";

                //Act
                BC = new BuildingController("C and t");
                bool stateSet = BC.SetCurrentState(invalidState);

                //Assert
                Assert.IsFalse(stateSet);                   //if state unchanged test passes.

            }

            //END OF SetCurrentState TESTS.
        }

        [TestFixture]   //Child test class 5 for BC SetCurrentState Tests.
        public class SetCurrentStateFollowsSTD          //(L2R1 + L2R2)
        {
            [TestCase("closed")]
            [TestCase("out of hours")]
            [TestCase("open")]
            [TestCase("fire drill")]
            [TestCase("fire alarm")]

            public void SetCurrentStateObeysSTD(string stateCases)
            {
                //Arrange 
                BuildingController BC;
                string state = stateCases;      //changes currentState instead of just checking whether or not they have been assigned.

                //Act
                BC = new BuildingController("C and t");
                BC.SetCurrentState(state);
                bool stateSet = BC.SetCurrentState("fire alarm");

                //Assert
                Assert.IsTrue(stateSet);

            }

            //END OF SetCurrentStateFollowsSTD.
        }

        [TestFixture] //Child test class 6 for BC SetCurrentState Tests.
        public class SecondConstructorTests     //(L2R3)
        {
            [Test]
            public void SecondConstructorInitialisesValue1()
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C and t", "open");
                string id = BC.GetBuildingID();

                //Assert
                Assert.AreEqual(id, "c and t");
            }

            [Test]
            public void SecondConstructorInitialisesValue2()
            {
                //Arrange
                BuildingController BC;

                //Act
                BC = new BuildingController("C and t", "open");
                string state = BC.GetCurrentState();

                //Assert
                Assert.AreEqual(state, "open");
            }

            [Test]
            public void SecondConstructorInitialisesCorrectState()
            {
                //Arrange
                BuildingController BC;

                //Act
                try
                {
                    BC = new BuildingController("C and t", "fire alarm");
                    Assert.Fail();
                }

                //Assert
                catch (Exception invalidState)
                {
                    Assert.AreEqual("Argument Exception: BuildingController can" +
                    " only be initialised to the following states 'open', 'closed', 'out of hours'", invalidState.Message);
                }

            }
        }

        //END OF BUILDING CONTROLLER TESTS.
    }
    //Interface Tests

    [TestFixture]   //all i can demonstrate my knowledge of mocks and stubs as i have no way of implementing the absract class needed for GetStatus()
    public class InterfaceTests         //(L3R1) 
    {
        [Test]
        public void BuildingControllerCorrectlyInitialisesInterfaces()
        {
            //Arrange

            IDoorManager mockDoorManager = Substitute.For<IDoorManager>();

            BuildingController BC = new BuildingController("c and t", mockDoorManager);

            //Act
            mockDoorManager.LockAllDoors();

            //Assert
            mockDoorManager.Received().LockAllDoors();
        }
    }

    //end of interface tests.
}