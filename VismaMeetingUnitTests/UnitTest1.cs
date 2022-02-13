using NUnit.Framework;
using Moq;
using VismaMeetingsTask.Models;
using VismaMeetingsTask.Services;
using VismaMeetingsTask.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace VismaMeetingUnitTests
{
    public class Tests
    {
        private IMeetingServices _services;
        private Mock<IMeetingRepository> _repository;
        private MeetingModel _meeting, _meeting2;
        private PersonMeetingModel _person, _person2, _person3;
        [SetUp]
        public void Setup()
        {
            var repo = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "/Files/";
            if (File.Exists(repo+ "Meetings.json"))
            {
                File.Delete(repo + "Meetings.json");
            }
            if(File.Exists(repo+ "PersonMeeting.json"))
            {
                File.Delete(repo + "PersonMeeting.json");
            }
            _repository = new Mock<IMeetingRepository>();
            _services = new MeetingServices(_repository.Object);
            _meeting = new MeetingModel("My meeting", "Me", "Good Meeting", "CodeMonkey", "Live", new DateTime(2022, 2, 12, 15, 50, 0), new DateTime(2022, 2, 12, 16, 0, 0));
            _meeting2 = new MeetingModel("My meeting2", "He", "Yes", "Hub", "InPerson", new DateTime(2021, 2, 12, 15, 50, 0), new DateTime(2021, 2, 12, 16, 0, 0));
            _person = new PersonMeetingModel("Me", "My meeting", new DateTime(2022, 2, 12, 15, 50, 0), new DateTime(2022, 2, 12, 16, 0, 0));
            _person2 = new PersonMeetingModel("John", "My meeting", new DateTime(2022, 2, 12, 15, 55, 0), new DateTime(2022, 2, 12, 16, 0, 0));
            _person3 = new PersonMeetingModel("He", "My meeting2", new DateTime(2021, 2, 12, 15, 50, 0), new DateTime(2021, 2, 12, 16, 0, 0));
        }

        [Test]
        public void AddMeetingToJson()
        {
            _repository.Setup(repository => repository.AddMeetingToJson(_meeting)).Returns(_meeting.Name);

            var result = _services.CreateAMeeting(_meeting);

            _repository.Verify(mock => mock.AddMeetingToJson(It.IsAny<MeetingModel>()), Times.Once());

            Assert.That(result.Equals(_meeting.Name));
        }

        [Test]
        public void AddMeetingToJsonWhenMeetingWithSameNameAlreadyExists()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel>() { _meeting});

            _repository.Verify(mock => mock.AddMeetingToJson(It.IsAny<MeetingModel>()),Times.Never());

            Assert.That(() => _services.CreateAMeeting(_meeting),
                        Throws.TypeOf<Exception>().With.Message.EqualTo("Meeting with this name already exists"));
        }

        [Test]
        public void DeleteMeetingFromJson()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel>() { _meeting });

            Assert.That(() => _services.DeleteAMeeting(_meeting.Name, _meeting.ResponsiblePerson),
                        Throws.Nothing);
        }

        [Test]
        public void DeleteMeetingWhenNoExistingMeeting()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel>() { _meeting });

            Assert.That(() => _services.DeleteAMeeting("New never seen meeting", "John"),
                        Throws.TypeOf<Exception>().With.Message.EqualTo("There is no meeting with the name New never seen meeting"));
        }

        [Test]
        public void AddPersonToMeeting()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting });

            Assert.That(() => _services.AddPersonToMeeting("John", "My meeting", new DateTime(2022, 2, 12, 15, 50, 0)),
                        Throws.Nothing);
        }

        [Test]
        public void AddPersonToMeetingWhenNoMeetings()
        {
            _repository.Setup(repository => repository.GetPeopleInMeeting()).Returns(new List<PersonMeetingModel> { null});

            Assert.That(() => _services.AddPersonToMeeting("John", "New never seen meeting", new DateTime(2022, 2, 12, 15, 50, 0)),
                        Throws.TypeOf<Exception>().With.Message.EqualTo("There are no meetings."));
        }

        [Test]
        public void ListAllMeetings()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting,_meeting2 });
            FilterModel filter = new FilterModel(FilterType.None);
            
            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == _repository.Object.GetMeetings().Count());
        }

        [Test]
        public void ListMeetingsWithDescriptionFilter()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting,_meeting2 });
            FilterModel filter = new FilterModel(FilterType.Description, "Yes");
            
            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == 1);
        }

        [Test]
        public void ListMeetingsWithResponsiblePersonFilter()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting, _meeting2 });
            FilterModel filter = new FilterModel(FilterType.ResponsiblePerson, "He");

            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == 1);
        }

        [Test]
        public void ListMeetingsCategoryFilter()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting, _meeting2 });
            FilterModel filter = new FilterModel(FilterType.Category, "CodeMonkey");

            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == 1);
        }

        [Test]
        public void ListMeetingsTypeFilter()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting, _meeting2 });
            FilterModel filter = new FilterModel(FilterType.Type, "InPerson");

            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == 1);
        }

        [Test]
        public void ListMeetingsDateFilter()
        {
            _repository.Setup(repository => repository.GetMeetings()).Returns(new List<MeetingModel> { _meeting, _meeting2 });
            FilterModel filter = new FilterModel(FilterType.Date, new DateTime(2021, 2, 11, 15, 50, 0), new DateTime(2021, 2, 13, 15, 50, 0));

            var result = _services.GetMeetings(filter);

            Assert.That(result.Count() == 1);
        }

    }
}