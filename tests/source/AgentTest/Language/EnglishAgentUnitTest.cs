using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AIAgent;
using AICore.Utils;
using NUnit.Framework;

namespace Tests.AgentTest.Language
{
    [TestFixture]
    public class EnglishAgentUnitTest : TestBase
    {
        #region Properties

        private Agent _agent;
        private Agent _agent2;
        private List<string> _observations;
        private List<string> _inputsThatMustReturnEmpty;
        private List<string> _inputsThatMustReturnNotEmpty;

        #endregion

        #region Initialization

        public EnglishAgentUnitTest()
        {
            _observations = new List<string>();
            _observations.Add("Going for a morning jog in the park");
            _observations.Add("Assembling a piece of furniture");
            _observations.Add("Trying a new recipe for dinner");
            _observations.Add("Participating in a local charity event");
            _observations.Add("Attending a live concert or show");
            _observations.Add("Visiting a museum or art gallery");
            _observations.Add("Painting or drawing a picture");
            _observations.Add("Taking a photography course");
            _observations.Add("Joining a sports team or club");
            _observations.Add("Volunteering at an animal shelter");
            _observations.Add("Organizing a yard sale");
            _observations.Add("Hiking a nearby nature trail");
            _observations.Add("Learning a new musical instrument");
            _observations.Add("Going on a road trip with friends");
            _observations.Add("Taking a dance class");
            _observations.Add("Watching a movie at the theater");
            _observations.Add("Attending a workout class at the gym");
            _observations.Add("Baking a cake or cookies");
            _observations.Add("Visiting a local farmer's market");
            _observations.Add("Planning a surprise party for a friend");
            _observations.Add("Starting a DIY home improvement project");
            _observations.Add("Writing a letter to a pen pal");
            _observations.Add("Exploring a new city or town");
            _observations.Add("Trying a new fitness activity, like yoga or Pilates");
            _observations.Add("Participating in a trivia night");
            _observations.Add("Attending a cooking or baking class");
            _observations.Add("Going for a bike ride around town");
            _observations.Add("Joining a book club");
            _observations.Add("Starting a garden in the backyard");
            _observations.Add("Learning a new language");
            _observations.Add("Visiting a theme park or amusement park");
            _observations.Add("Going on a weekend camping trip");
            _observations.Add("Attending a sports game or event");
            _observations.Add("Participating in a neighborhood clean-up day");
            _observations.Add("Playing a board game or card game with friends");
            _observations.Add("Going ice skating or roller skating");
            _observations.Add("Trying a new type of cuisine at a local restaurant");
            _observations.Add("Completing a jigsaw puzzle");
            _observations.Add("Visiting an escape room with friends");
            _observations.Add("Going fishing at a nearby lake or river");
            _observations.Add("Attending a wine tasting or brewery tour");
            _observations.Add("Joining a pottery or art class");
            _observations.Add("Going on a scenic drive or walk");
            _observations.Add("Participating in a karaoke night");
            _observations.Add("Visiting a zoo or aquarium");
            _observations.Add("Attending a local community event or festival");
            _observations.Add("Going for a swim at the local pool or beach");
            _observations.Add("Taking a scenic train ride");
            _observations.Add("Participating in a group painting class");
            _observations.Add("Visiting a nearby national park or monument");
            _observations.Add("Organizing a picnic in the park");
            _observations.Add("Trying out a new outdoor sport like paddleboarding or kayaking");
            _observations.Add("Taking a meditation or mindfulness class");
            _observations.Add("Going on a guided city tour");
            _observations.Add("Attending a live theater performance or play");
            _observations.Add("Signing up for a woodworking or metalworking class");
            _observations.Add("Trying out a new hobby, like knitting or crocheting");
            _observations.Add("Planning a themed dinner party with friends");
            _observations.Add("Exploring a new hiking trail with friends");
            _observations.Add("Watching a movie marathon or binge-watching a TV series");
            _observations.Add("Participating in a local charity run or walk");
            _observations.Add("Visiting a historic site or landmark");
            _observations.Add("Going on a hot air balloon ride");
            _observations.Add("Taking a self-defense or martial arts class");
            _observations.Add("Joining a choir or singing group");
            _observations.Add("Trying your hand at geocaching");
            _observations.Add("Attending a poetry reading or open mic night");
            _observations.Add("Going to a comedy show or stand-up performance");
            _observations.Add("Learning to create origami or paper crafts");
            _observations.Add("Exploring an indoor climbing gym");
            _observations.Add("Joining a local community garden");
            _observations.Add("Taking a flower arranging or gardening class");
            _observations.Add("Visiting a planetarium or observatory");
            _observations.Add("Trying a new workout class, like kickboxing or Zumba");
            _observations.Add("Going on a birdwatching or wildlife tour");
            _observations.Add("Participating in a community theater production");
            _observations.Add("Going to a drive-in movie theater");
            _observations.Add("Trying out a new water sport, like surfing or sailing");
            _observations.Add("Learning how to make candles or soap");
            _observations.Add("Visiting a butterfly or botanical garden");
            _observations.Add("Attending a lecture or workshop on a topic of interest");
            _observations.Add("Going on a stargazing or nighttime nature walk");
            _observations.Add("Trying out a new virtual reality experience");
            _observations.Add("Participating in a themed race or obstacle course");
            _observations.Add("Going to a local arcade or gaming center");
            _observations.Add("Attending a live recording of a podcast or radio show");
            _observations.Add("Taking a mixology or cocktail-making class");
            _observations.Add("Going on a treasure hunt or scavenger hunt with friends");
            _observations.Add("Participating in a group or team-building activity");
            _observations.Add("Joining a chess club or strategy game group");
            _observations.Add("Taking a cooking class focused on a specific cuisine or technique");
            _observations.Add("Going on a guided nature walk or botanical tour");
            _observations.Add("Trying out a new extreme sport, like bungee jumping or skydiving");
            _observations.Add("Visiting an art studio to create your own masterpiece");
            _observations.Add("Attending a cultural event, like a traditional dance performance");
            _observations.Add("Going to a food or drink festival");
            _observations.Add("Taking a class or workshop on sustainable living");
            _observations.Add("Participating in a historical reenactment or event");
            _observations.Add("Going on a guided ghost or paranormal tour");
            _observations.Add("Joining a local group focused on environmental conservation or activism");
            _observations.Add("Learn the basics of a new programming language");
            _observations.Add("Attend a pottery painting session");
            _observations.Add("Try your hand at calligraphy or hand lettering");
            _observations.Add("Go on a guided foraging walk to learn about edible plants");
            _observations.Add("Visit a flea market or antique shop");
            _observations.Add("Write a short story or poem and share it with friends");
            _observations.Add("Learn about local history by visiting a small museum");
            _observations.Add("Go on a mural or street art tour in your city");
            _observations.Add("Host a themed costume party for friends");
            _observations.Add("Take a glassblowing or stained glass workshop");
            _observations.Add("Watch a live stream of a space launch");
            _observations.Add("Learn to juggle or perform basic magic tricks");
            _observations.Add("Visit a local farm to learn about agriculture and sustainable practices");
            _observations.Add("Listen to a new podcast or audiobook on a topic of interest");
            _observations.Add("Make your own piece of jewelry, like a bracelet or necklace");
            _observations.Add("Attend a silent disco or outdoor movie night");
            _observations.Add("Learn how to compost and start a compost pile at home");
            _observations.Add("Try out a new form of meditation or mindfulness practice");
            _observations.Add("Write a letter to your future self and seal it in an envelope");
            _observations.Add("Attend a local storytelling event or participate as a storyteller");
            _observations.Add("Visit a public garden or arboretum");
            _observations.Add("Attend a live cooking or baking demonstration");
            _observations.Add("Go on a guided food or culinary tour in your city");
            _observations.Add("Learn how to sew, knit, or crochet and create a handmade gift");
            _observations.Add("Join a community group focused on a cause you're passionate about");
            _observations.Add("Participate in a blindfolded taste testing with friends");
            _observations.Add("Learn basic self-care practices, like deep breathing or stretching");
            _observations.Add("Host a 'paint and sip' night at home with friends");
            _observations.Add("Visit a local distillery or cidery for a tour and tasting");
            _observations.Add("Take a metal detecting adventure at a beach or park");
            _observations.Add("Try a new type of group exercise class, like barre or aerial yoga");
            _observations.Add("Attend an outdoor theater or Shakespeare in the Park performance");
            _observations.Add("Make a vision board to visualize your goals and dreams");
            _observations.Add("Visit a historic battlefield or site in your area");
            _observations.Add("Attend a stargazing event or meteor shower viewing party");
            _observations.Add("Learn how to play a new card or tabletop game");
            _observations.Add("Participate in a local or online film festival");
            _observations.Add("Try your hand at a new craft, like embroidery or woodworking");
            _observations.Add("Attend a local author reading or book signing event");
            _observations.Add("Go on a guided eco-tour or nature walk in your area");
            _observations.Add("Visit an alpaca or llama farm for a unique experience");
            _observations.Add("Learn a new artistic skill, like printmaking or sculpture");
            _observations.Add("Attend a cultural fair or international food festival");
            _observations.Add("Host a game night with a mix of classic and new board games");
            _observations.Add("Learn about your family history or create a family tree");
            _observations.Add("Try your hand at an outdoor adventure sport, like rock climbing or zip-lining");
            _observations.Add("Attend a local lecture or TEDx event");
            _observations.Add("Visit an exotic pet store or reptile exhibit");
            _observations.Add("Go on a scenic photography walk to capture your surroundings");
            _observations.Add("Participate in a themed pub crawl or bar hop with friends");
            _observations.Add("Take a salsa, tango, or swing dancing class");
            _observations.Add("Volunteer at a local animal shelter or rescue organization");
            _observations.Add("Create your own vision board or bucket list");
            _observations.Add("Build a model airplane, car, or ship from a kit");
            _observations.Add("Attend a trivia night at a local pub or café");
            _observations.Add("Visit an escape room with friends or family");
            _observations.Add("Listen to a live orchestra, symphony, or chamber music performance");
            _observations.Add("Learn how to make candles or soap at home");
            _observations.Add("Host a themed movie marathon with friends");
            _observations.Add("Attend a lecture, workshop, or conference on a topic you're passionate about");
            _observations.Add("Explore a local nature reserve or wildlife sanctuary");
            _observations.Add("Try out a new watersport, like paddleboarding, kayaking, or canoeing");
            _observations.Add("Go on a guided ghost tour or haunted history walk");
            _observations.Add("Participate in a charity walk or run event");
            _observations.Add("Learn how to make your favorite cocktail or mocktail at home");
            _observations.Add("Visit a butterfly or insect conservatory");
            _observations.Add("Attend a unique performance, like a mime or circus act");
            _observations.Add("Host a cooking or baking competition with friends");
            _observations.Add("Participate in a beach clean-up or tree planting event");
            _observations.Add("Attend a live comedy show or stand-up performance");
            _observations.Add("Explore a nearby cave or cavern on a guided tour");
            _observations.Add("Learn a new instrument or improve your skills on one you already play");
            _observations.Add("Host a DIY spa day at home with friends");
            _observations.Add("Attend a local renaissance fair or medieval festival");
            _observations.Add("Create a scrapbook or photo album to document your memories");
            _observations.Add("Visit a virtual reality arcade or gaming center");
            _observations.Add("Take a hot air balloon ride or helicopter tour");
            _observations.Add("Participate in a sandcastle-building competition at the beach");
            _observations.Add("Attend a poetry slam or spoken word performance");
            _observations.Add("Learn how to make your own pottery or ceramics");
            _observations.Add("Visit a nearby lighthouse or historical landmark");
            _observations.Add("Take a guided tour of a local brewery or winery");
            _observations.Add("Attend a local sports game or match, like baseball, basketball, or soccer");
            _observations.Add("Go horseback riding or take riding lessons");
            _observations.Add("Participate in a community garden or urban farming project");
            _observations.Add("Take a scenic train ride or day trip");
            _observations.Add("Learn the basics of a new language");
            _observations.Add("Attend a live art performance, like a dance or theater show");
            _observations.Add("Volunteer at a soup kitchen or food pantry");
            _observations.Add("Go indoor or outdoor rock climbing");
            _observations.Add("Participate in a local or online photography contest");
            _observations.Add("Visit a nearby aquarium or marine center");
            _observations.Add("Take a class or workshop on gardening, landscaping, or plant care");
            _observations.Add("Learn about the history of a nearby town or city on a guided tour");
            _observations.Add("Attend a local car show, air show, or boat show");
            _observations.Add("Participate in a color run, mud run, or obstacle course race");
            _observations.Add("Go on a weekend getaway or short road trip to a nearby destination");
            _observations.Add("Try out a new fitness class, like pilates, tai chi, or martial arts");
            _observations.Add("Attend a paint-your-own pottery or ceramics studio");
            _observations.Add("Go on a guided birdwatching or wildlife spotting tour");

            _inputsThatMustReturnEmpty = new List<string>();
            _inputsThatMustReturnEmpty.Add("Who is Elon Musk?");
            _inputsThatMustReturnEmpty.Add("Who is Donald Trump?");
            _inputsThatMustReturnEmpty.Add("Who is Shakira Mebarak?");
            _inputsThatMustReturnEmpty.Add("What is the derivative of 2x^2 + 5x - 7?");
            _inputsThatMustReturnEmpty.Add("What is the sum of the interior angles of a pentagon?");
            _inputsThatMustReturnEmpty.Add("What is the area of a circle with a radius of 5?");
            _inputsThatMustReturnEmpty.Add("Who is Albert Einstein?");
            _inputsThatMustReturnEmpty.Add("What is the molecular formula of glucose?");
            _inputsThatMustReturnEmpty.Add("What is the square root of 64?");
            _inputsThatMustReturnEmpty.Add("What is the chemical symbol for gold?");
            _inputsThatMustReturnEmpty.Add("Who was the first man on the moon?");
            _inputsThatMustReturnEmpty.Add("What is the largest planet in our solar system?");
            _inputsThatMustReturnEmpty.Add("What is the value of pi to 3 decimal places?");
            _inputsThatMustReturnEmpty.Add("Who was the first president of the United States?");
            _inputsThatMustReturnEmpty.Add("What is the atomic number of carbon?");
            _inputsThatMustReturnEmpty.Add("Who is the author of The Catcher in the Rye?");
            _inputsThatMustReturnEmpty.Add("What is the formula for the volume of a cylinder?");
            _inputsThatMustReturnEmpty.Add("Who invented the telephone?");
            _inputsThatMustReturnEmpty.Add("What is the speed of light in a vacuum?");
            _inputsThatMustReturnEmpty.Add("What is the chemical formula for water?");
            _inputsThatMustReturnEmpty.Add("Who was the first woman in space?");
            _inputsThatMustReturnEmpty.Add("What is the largest mammal on Earth?");
            _inputsThatMustReturnEmpty.Add("What is the Pythagorean theorem?");
            _inputsThatMustReturnEmpty.Add("Who is known as the Father of Modern Physics?");
            _inputsThatMustReturnEmpty.Add("What is the density of water at 4 degrees Celsius?");
            _inputsThatMustReturnEmpty.Add("What is the capital city of Spain?");
            _inputsThatMustReturnEmpty.Add("What is the formula for finding the area of a triangle?");
            _inputsThatMustReturnEmpty.Add("Who invented the steam engine?");
            _inputsThatMustReturnEmpty.Add("What is the gravitational constant?");
            _inputsThatMustReturnEmpty.Add("What is the chemical symbol for sodium?");
            _inputsThatMustReturnEmpty.Add("Who discovered the laws of motion?");
            _inputsThatMustReturnEmpty.Add("What is the longest river in the world?");
            _inputsThatMustReturnEmpty.Add("What is the formula for calculating the circumference of a circle?");
            _inputsThatMustReturnEmpty.Add("What is the atomic mass of hydrogen?");
            _inputsThatMustReturnEmpty.Add("What is the process of converting light energy into chemical energy called?");
            _inputsThatMustReturnEmpty.Add("What is the boiling point of water in Fahrenheit?");
            _inputsThatMustReturnEmpty.Add("What is the chemical formula for carbon dioxide?");
            _inputsThatMustReturnEmpty.Add("Who was the first person to reach the South Pole?");
            _inputsThatMustReturnEmpty.Add("What is the smallest planet in our solar system?");
            _inputsThatMustReturnEmpty.Add("What is the square root of 169?");
            _inputsThatMustReturnEmpty.Add("Who was the 16th president of the United States?");
            _inputsThatMustReturnEmpty.Add("What is the atomic number of oxygen?");
            _inputsThatMustReturnEmpty.Add("What is the process by which cells reproduce called?");

            _inputsThatMustReturnNotEmpty = new List<string>();
            _inputsThatMustReturnNotEmpty.Add("How's everything going for you?");
            _inputsThatMustReturnNotEmpty.Add("Did you have a good night's sleep?");
            _inputsThatMustReturnNotEmpty.Add("What are your plans for the day?");
            _inputsThatMustReturnNotEmpty.Add("How's your mood today?");
            _inputsThatMustReturnNotEmpty.Add("What have you been up to lately?");
            _inputsThatMustReturnNotEmpty.Add("How's your week been so far?");
            _inputsThatMustReturnNotEmpty.Add("Did you get some time to relax recently?");
            _inputsThatMustReturnNotEmpty.Add("Are you enjoying the weather today?");
            _inputsThatMustReturnNotEmpty.Add("Have you done anything exciting lately?");
            _inputsThatMustReturnNotEmpty.Add("How have you been keeping yourself entertained?");
            _inputsThatMustReturnNotEmpty.Add("How's your family doing?");
            _inputsThatMustReturnNotEmpty.Add("Have you been eating well lately?");
            _inputsThatMustReturnNotEmpty.Add("Did you catch up with any friends recently?");
            _inputsThatMustReturnNotEmpty.Add("What's your favorite part of the day?");
            _inputsThatMustReturnNotEmpty.Add("How are you feeling today?");
            _inputsThatMustReturnNotEmpty.Add("Did anything interesting happen to you today?");
            _inputsThatMustReturnNotEmpty.Add("What's on your mind at the moment?");
            _inputsThatMustReturnNotEmpty.Add("Are you looking forward to anything?");
            _inputsThatMustReturnNotEmpty.Add("Have you been able to get some exercise lately?");
            _inputsThatMustReturnNotEmpty.Add("What's been the highlight of your week?");
            _inputsThatMustReturnNotEmpty.Add("Are you keeping up with your hobbies?");
            _inputsThatMustReturnNotEmpty.Add("How are things going at work or school?");
            _inputsThatMustReturnNotEmpty.Add("Have you learned anything new recently?");
            _inputsThatMustReturnNotEmpty.Add("How are you managing stress these days?");
            _inputsThatMustReturnNotEmpty.Add("Did you watch any good movies or shows lately?");
            _inputsThatMustReturnNotEmpty.Add("What have you been listening to lately?");
            _inputsThatMustReturnNotEmpty.Add("How's your current book, if you're reading one?");
            _inputsThatMustReturnNotEmpty.Add("Did you try any new recipes recently?");
            _inputsThatMustReturnNotEmpty.Add("How are you staying connected with loved ones?");
            _inputsThatMustReturnNotEmpty.Add("Are you planning any trips or vacations?");
            _inputsThatMustReturnNotEmpty.Add("Have you attended any interesting events recently?");
            _inputsThatMustReturnNotEmpty.Add("Did you have any recent accomplishments?");
            _inputsThatMustReturnNotEmpty.Add("How's the traffic today, if you've been out?");
            _inputsThatMustReturnNotEmpty.Add("Are you keeping up with current events?");
            _inputsThatMustReturnNotEmpty.Add("What's something that made you smile today?");
            _inputsThatMustReturnNotEmpty.Add("How's the local weather treating you?");
            _inputsThatMustReturnNotEmpty.Add("Have you discovered any new hobbies or interests?");
            _inputsThatMustReturnNotEmpty.Add("What's the most recent thing that surprised you?");
            _inputsThatMustReturnNotEmpty.Add("Have you made any new friends lately?");
            _inputsThatMustReturnNotEmpty.Add("Are there any new trends that you're enjoying?");
            _inputsThatMustReturnNotEmpty.Add("How are you staying healthy and active?");
            _inputsThatMustReturnNotEmpty.Add("Did you have any recent memorable experiences?");
            _inputsThatMustReturnNotEmpty.Add("Are you working on any personal projects?");
            _inputsThatMustReturnNotEmpty.Add("Have you been to any new places recently?");
            _inputsThatMustReturnNotEmpty.Add("What's been your favorite meal this week?");
            _inputsThatMustReturnNotEmpty.Add("How are you spending your free time?");
            _inputsThatMustReturnNotEmpty.Add("Have you made any changes to your daily routine?");
            _inputsThatMustReturnNotEmpty.Add("Are you trying anything new to stay motivated?");
            _inputsThatMustReturnNotEmpty.Add("How's your overall outlook on life these days?");
        }

        [SetUp]
        public void Setup()
        {
            _agent = new Agent();
            _agent2 = new Agent();
            _agent.Initialize("John Lin", "en");
            _agent2.Initialize("Sam Moore", "en");
        }

        private async Task SetupNPC1()
        {
            await _agent.AddMemory("John Lin is a pharmacy merchant in Willow Market and Pharmacy who loves to help people", 8);
            await _agent.AddMemory("John Lin is always looking for ways to make the process to provide medication to his clients", 8);
            await _agent.AddMemory("John Lin lives with his wife, Mei Lin, who is a university professor, and his son, Eddy Lin, who is a music theory student", 8);
            await _agent.AddMemory("John Lin loves his family a lot of him", 8);
            await _agent.AddMemory("John Lin has known the old couple next door, Sam Moore and Jennifer Moore, for some years", 8);
            await _agent.AddMemory("John Lin thinks that Sam Moore is a kind and pleasant man", 8);
            await _agent.AddMemory("John Lin meets his neighbor, Yuriko Yamamoto, well", 8);
            await _agent.AddMemory("John Lin knows of his his neighbors, Tamara Taylor and Carmen Ortiz, but he has not met them before", 8);
            await _agent.AddMemory("John Lin and Tom Moreno they are colleagues at The Willows Market and Pharmacy", 8);
            await _agent.AddMemory("John Lin and Tom Moreno are friends and they like discuss local politics together", 8);
            await _agent.AddMemory("John Lin knows the Moreno family somewhat well — husband Tom Moreno and wife Jane Moreno", 8);
        }

        private async Task SetupNPC2()
        {
            await _agent2.AddMemory("Sam Moore is a retired engineer who enjoys spending time in his garden and woodworking in his workshop. He has a wealth of knowledge from his years of experience and is always happy to share it with others", 8);
            await _agent2.AddMemory("Sam Moore and his wife, Jennifer Moore, have been living in their cozy home for over 30 years, and they have seen many neighbors come and go, but they have a special fondness for John Lin and his family", 8);
            await _agent2.AddMemory("Sam Moore appreciates John Lin's helpful nature and dedication to the community, often mentioning to friends and family how fortunate they are to have such a caring neighbor", 8);
            await _agent2.AddMemory("Sam Moore is an active member of the neighborhood association and often collaborates with John Lin on projects to improve the area for all residents", 8);
            await _agent2.AddMemory("Sam Moore enjoys playing chess and often invites John Lin over for a friendly match, sharing stories and wisdom as they play", 8);
            await _agent2.AddMemory("Sam Moore has a great sense of humor, and his laughter can often be heard echoing throughout the neighborhood, especially when he's chatting with John Lin", 8);
            await _agent2.AddMemory("Sam Moore has a deep love for classical music and has found a connection with John Lin's son, Eddy, who is a music theory student. Sam enjoys attending Eddy's recitals and discussing music with him", 8);
            await _agent2.AddMemory("Sam Moore is a proud grandfather and often shares his experiences and advice with John Lin, who values Sam's wisdom and guidance", 8);
            await _agent2.AddMemory("Sam Moore, despite his age, is always eager to learn new things and has taken an interest in understanding more about pharmacy and medicine from John Lin, who is always happy to share his expertise", 8);
            await _agent2.AddMemory("Sam Moore and his wife, Jennifer, often invite John Lin and his family over for dinner or barbecues, strengthening their bond as neighbors and friends, and celebrating their shared love for their close-knit community", 8);
            await _agent2.AddMemory("Sam Moore is a proud grandfather and often shares his experiences and advice with John Lin, who values Sam's wisdom and guidance", 8);
        }

        #endregion

        #region Tests
        
        [Test]
        public async Task TestEmbeddingVector()
        {
            //Test observation
            var observation = "Go on a guided birdwatching or wildlife spotting tour";
            var observationEmbedding = await _agent.GetObservationEmbedding(observation);
            var wordEmbedding = await _agent.GetObservationEmbedding("Animals");
            var result = MathUtils.CalculateCosineSimilarity(observationEmbedding, wordEmbedding);
            Assert.Greater(result, 0.75f, "The similarity between the observation and the word is not high enough");
        }


        [Test]
        public async Task TestRequestImportance()
        {
            //Test observation
            Console.WriteLine("Starting test of " + _observations.Count + " RequestImportance requests\n");
            for (int i = 0; i < _observations.Count; i++)
            {
                var score = await _agent.GetObservationImportance(_observations[i]);
                Console.WriteLine("ID: " + (i + 1) + " - " + _observations[i] + " - Score: " + score);
            }
        }

        [Test]
        public async Task TestMemory()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < _observations.Count; i++)
            {
                await _agent.AddMemory(_observations[i]);
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed {0}: {1} s",0, stopwatch.Elapsed.TotalSeconds);

            stopwatch.Restart();
            stopwatch.Start();
            //Now get the top 20 memories
            var observationEmbedding = await _agent.GetObservationEmbedding("Do you usually do a lot of sport?");
            var memories = _agent.MemoryRetrieval(observationEmbedding);
            foreach (var memory in memories)
            {
                Console.WriteLine("ID: " + memory.Id + " - " + memory.Data + " - Total Score: " +
                                  memory.TotalScore + " - Recency: " + memory.RecencyScore + " - Importance: " + 
                                  memory.ImportanceScore + " - Relevance: " + memory.RelevanceScore);
            }
            Console.WriteLine("Time elapsed {0}: {1} s",1, stopwatch.Elapsed.TotalSeconds);
        }

        [Test]
        public async Task TestAgentMessage()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Console.WriteLine("Start TestAgentMessage");
            Console.WriteLine("Uploading memories to the agent");
            await SetupNPC1();
            stopwatch.Stop();
            Console.WriteLine("Memories uploaded, time elapsed: " + stopwatch.Elapsed.TotalSeconds + " seconds");
            
            stopwatch.Restart();
            stopwatch.Start();
            
            Console.WriteLine("Starting conversation");
            var question = "Hello, how are you?";
            var response = (await _agent.RequestAgentMessage(question)).message;
            Console.WriteLine("Test question: " + question);
            Console.WriteLine("Agent response: " + response);
            question = "Who are you?";
            response = (await _agent.RequestAgentMessage(question)).message;
            Console.WriteLine("Test question: " + question);
            Console.WriteLine("Agent response: " + response);
            Console.WriteLine("Time elapsed: " + stopwatch.Elapsed.TotalSeconds + " seconds");
        }

        [Test]
        public async Task TestAgentMessageOutOfContext()
        {
            await SetupNPC1();
            foreach (var it in _inputsThatMustReturnEmpty)
            {
                await Task.Delay(150);
                var response = (await _agent.RequestAgentMessage(it)).message;
                Console.WriteLine("Test question: " + it);
                Console.WriteLine("Agent response: " + response);
                Assert.IsTrue(response.Contains("Sorry") || response.Contains("don't know"));
                await Task.Delay(350);
            }
        }
        
        [Test]
        public async Task TestAgentMessageInContext()
        {
            await SetupNPC1();
            foreach (var it in _inputsThatMustReturnNotEmpty)
            {
                await Task.Delay(150);
                var response = (await _agent.RequestAgentMessage(it)).message;
                Console.WriteLine("Test question: " + it);
                Console.WriteLine("Agent response: " + response);
                Assert.AreNotEqual(string.Empty, response, "The response should not result in a empty string");
                await Task.Delay(350);
            }
        }

        [Test]
        public async Task TestMessageBetweenAgents()
        {
            Console.WriteLine("Start TestMessageBetweenAgents");
            
            Console.WriteLine("Uploading memories to the agent John Lin");
            await SetupNPC1();
            
            Console.WriteLine("Uploading memories to the agent Sam Moore");
            await SetupNPC2();

            var maxTurns = 10;
            var turn = 0;
            var question = "Hello I'm John Lin, how are you?";
            Console.WriteLine("John Lin: " + question);
            while (turn < maxTurns)
            {
                var resultA = await _agent2.RequestAgentMessage(question, "John Lin");
                question = resultA.message;
                Console.WriteLine("Sam Moore: " + question + " - Emotion: " + resultA.emotion);
                var resultB = await _agent.RequestAgentMessage(question, "Sam Moore");
                question = resultB.message;
                Console.WriteLine("John Lin: " + question + " - Emotion: " + resultB.emotion);
                turn++;
            }
            Console.WriteLine("Agent conversation finished\n");
            Console.WriteLine("Agent John Lin thoughts: ");
            (await _agent.GetReflectAgentThoughts()).ForEach(it => Console.WriteLine(it));
            Console.WriteLine("\nAgent Sam Moore thoughts: ");
            (await _agent2.GetReflectAgentThoughts()).ForEach(it => Console.WriteLine(it));
        }
        
        #endregion
    }
}