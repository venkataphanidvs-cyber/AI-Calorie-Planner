using CaloriePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaloriePlanner.Services
{
    public class CalorieService
    {
        public double CalculateBMR(User user)
        {
            int s = user.Gender.ToLower() == "male" ? 5 : -161;

            return (10 * user.Weight) +
                   (6.25 * user.Height) -
                   (5 * user.Age) + s;
        }

        public double CalculateTDEE(double bmr, double activityFactor)
        {
            return bmr * activityFactor;
        }
    }
}
