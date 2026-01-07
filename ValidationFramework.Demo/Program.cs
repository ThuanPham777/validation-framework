using System;
using ValidationFramework.Demo.Demos;

namespace ValidationFramework.Demo
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            DemoHelpers.PrintHeader("ValidationFramework - Interactive Demo");
            Console.WriteLine("Welcome to ValidationFramework Interactive Demo!");
            Console.WriteLine("Each demo will guide you through data entry and validation.\n");

            bool continueDemo = true;

            while (continueDemo)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Demo1_AttributeBased.Run();
                        break;
                    case "2":
                        Demo2_FluentValidation.Run();
                        break;
                    case "3":
                        Demo3_HybridApproach.Run();
                        break;
                    case "4":
                        Demo4_CustomValidators.Run();
                        break;
                    case "5":
                        Demo5_ValidatorGroups.Run();
                        break;
                    case "6":
                        Demo6_DelegateValidators.Run();
                        break;
                    case "7":
                        Demo7_ExtensionMethods.Run();
                        break;
                    case "8":
                        RunAllDemos();
                        break;
                    case "0":
                        continueDemo = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }

                if (continueDemo && choice != "0")
                {
                    Console.WriteLine("\n" + new string('=', 70));
                    Console.Write("Press any key to return to menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            DemoHelpers.PrintHeader("Thank You!");
            Console.WriteLine("Thank you for trying ValidationFramework!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ShowMenu()
        {
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("  DEMO MENU");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();
            Console.WriteLine("  1. Attribute-Based Validation");
            Console.WriteLine("  2. Fluent Validation API");
            Console.WriteLine("  3. Hybrid Approach (Attributes + Fluent)");
            Console.WriteLine("  4. Custom Validators");
            Console.WriteLine("  5. Validator Groups");
            Console.WriteLine("  6. Delegate Validators");
            Console.WriteLine("  7. Extension Methods");
            Console.WriteLine();
            Console.WriteLine("  8. Run All Demos (Sequential)");
            Console.WriteLine("  0. Exit");
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.Write("\nEnter your choice (0-8): ");
        }

        static void RunAllDemos()
        {
            DemoHelpers.PrintHeader("Running All Demos");
            Console.WriteLine("Running all demos sequentially...\n");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            Console.Clear();

            Demo1_AttributeBased.Run();
            PauseBetweenDemos();

            Demo2_FluentValidation.Run();
            PauseBetweenDemos();

            Demo3_HybridApproach.Run();
            PauseBetweenDemos();

            Demo4_CustomValidators.Run();
            PauseBetweenDemos();

            Demo5_ValidatorGroups.Run();
            PauseBetweenDemos();

            Demo6_DelegateValidators.Run();
            PauseBetweenDemos();

            Demo7_ExtensionMethods.Run();

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("All demos completed!");
        }

        static void PauseBetweenDemos()
        {
            Console.WriteLine("\n" + new string('-', 70));
            Console.Write("Press any key to continue to next demo...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
