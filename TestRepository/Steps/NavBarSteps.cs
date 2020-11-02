using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace TestRepository.Steps
{
    [Binding]
    public class NavBarSteps
    {
        [Given(@"I navigate to the Home Page")]
        public void GivenINavigateToTheHomePage()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
