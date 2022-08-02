using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace Selenium_Automation
{
    class Program
    {
        static void Main(string[] args)

        {
            string keywordname = "keywordname.txt";
            //creiamo il driver
            IWebDriver driver = new ChromeDriver("C:\\Users\\Utente\\source\\repos\\Selly\\Selly\\bin\\Debug\\net6.0");
            // This will open up the URL
            driver.Navigate().GoToUrl("https://www.linkedin.com/login/it?fromSignIn=true&trk=guest_homepage-basic_nav-header-signin");
            Thread.Sleep(1500);
            // famo il login
            IWebElement ele = driver.FindElement(By.Name("session_key"));
            //enter the value in the google search text box  
            string filename = "config.cfg";
            var lines = File.ReadLines(filename);
            List<string> config = new List<string>();
            int c = 0;

            var lines2 = File.ReadLines(filename);
            foreach (var line2 in lines)
            {
                config.Add(line2);
            }
            //inizializziamo il file
            ele.SendKeys(config[0]);
            Thread.Sleep(1000);
            ele = driver.FindElement(By.Name("session_password"));
            ele.SendKeys(config[1]);
            Thread.Sleep(1000);

            IWebElement ele1 = driver.FindElement(By.XPath("/html/body/div[1]/main/div[3]/div[1]/form/div[3]/button"));
            Thread.Sleep(1500);
            // clicka
            ele1.Click();
          

            Console.WriteLine("Quante pagine vogliamo usare:  ");
            int contoforach = Int32.Parse(Console.ReadLine());

            // Read the file and display it line by line.  
            foreach (string line in System.IO.File.ReadLines("keywords.txt"))
            {
                int contatore = 1;
                int counter = 0;
                int contapag = 0;
                int pagine = 10;
                int paginainiziale = 1;

                //System.Console.WriteLine("Dopo +"+ contatore);
                //System.Console.WriteLine(contapag);
                driver.Navigate().GoToUrl("https://www.linkedin.com/search/results/people/?keywords=" + line + "&origin=SWITCH_SEARCH_VERTICAL=" + paginainiziale);
                Thread.Sleep(1500);
                string nuovolink = null;

                while (contapag < contoforach)
                {
                    Thread.Sleep(1500);
                    contatore = contatore + 1;
                    System.Console.WriteLine(contatore); ;
                    // System.Console.WriteLine(contatore);
                    //System.Console.WriteLine(contapag);
                    try
                    {
                        if (contatore > 9)
                        {

                            paginainiziale = paginainiziale + 1;
                            nuovolink = "https://www.linkedin.com/search/results/people/?keywords=disegnatore%20navale&origin=SWITCH_SEARCH_VERTICAL&page=" + paginainiziale;
                            contatore = 1;
                        }
                    }
                    catch
                    {


                        var body = driver.FindElement(By.CssSelector("[type='button']:nth-child(3) .artdeco-button__text"));

                        Actions builder = new Actions(driver);

                        builder
                            .MoveToElement(body, 0, 0)
                            .Click()
                            .Build()
                            .Perform();
                    }
                    //ricerca iterata ogni keyword

                    string search = "https://www.linkedin.com/search/results/people/?keywords=" + line + "&origin=SWITCH_SEARCH_VERTICAL=" + paginainiziale;
                    if (nuovolink != null)
                    {
                        search = nuovolink;
                    }
                    System.Console.WriteLine(line);
                    driver.Navigate().GoToUrl(search);
                    Thread.Sleep(3500);
                    ele1 = driver.FindElement(By.XPath("/html/body/div[5]/div[3]/div[2]/div/div[1]/main/div/div/div[1]/ul/li[" + contatore + "]/div/div/div[2]/div[1]/div[1]/div/span[1]/span/a/span/span[1]"));
                    // click on the Google search button  
                    //System.Console.WriteLine(ele1.Text);
                    string nome = ele1.Text;
                    ele1.Click();
                    //esempio di come prendere cose, XPATH dell'oggetto, poi salvi su stringa in questo caso.
                    Thread.Sleep(1500);
                    ele1 = driver.FindElement(By.XPath("/html/body/div[5]/div[3]/div/div/div[2]/div/div/main/section[1]/div[2]/div[2]/div[1]/div[2]"));
                    string lavoro = ele1.Text;
                    //System.Console.WriteLine(ele1.Text);
                    ele1 = driver.FindElement(By.XPath("/html/body/div[5]/div[3]/div/div/div[2]/div/div/main/section[1]/div[2]/div[2]/div[2]"));
                    string localita = ele1.Text;
                    //System.Console.WriteLine(ele1.Text);
                    string analisi = driver.PageSource;
                    int count = analisi.Split(line).Length - 1;
                    //creiamo il profilo + conta 
                    string persona = nome + ";" + lavoro + ";" + localita + " ; Score : " + count.ToString();

                    //leggiamo valori customizzati
                    List<string> custom = new List<string>();
                    int l = 0;
                    try
                    {
                        var linez2 = File.ReadLines(keywordname);
                        foreach (var linez in linez2)
                        {
                            ele1 = driver.FindElement(By.XPath(linez)); //cerca xpath custom
                            persona = persona + " ; " + ele1.Text; //aggiunge dato
                        }
                    }
                    catch
                    {
                        //non gestione delle eccezioni
                    }


                    //formattiamo i dati correttamente
                    persona = persona.Replace("Informazioni di contatto", driver.Url);
                    System.Console.WriteLine(persona);
                    var date = DateTime.Now.ToString("ddd M-dd-yy");
                    string newFileName = date + @".txt";
                    string fileName = Directory.GetCurrentDirectory() + "//result//" + newFileName;
                    if (!File.Exists(fileName))
                    {
                        using (var myFile = File.Create(fileName))
                        {
                            // interact with myFile here, it will be disposed automatically
                        }
                    }



                    //salviamo i dati nel nostro txt magico di oiutput
                    string appendText = persona + Environment.NewLine;
                    File.AppendAllText(fileName, appendText);

                    contoforach = contoforach + 1;
                    // contatore = contoforach + 1;
                    // System.Console.WriteLine("Prima " + contatore);
                    driver.Navigate().GoToUrl(search);
                }

            }


        }




    }

}


