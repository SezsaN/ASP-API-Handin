﻿namespace Infrastructure.Models;

public class SubscriberDto
{
    public string Email { get; set; } = null!;

    public bool DailyNewsletter { get; set; } = false;

    public bool AdvertisingUpdates { get; set; } = false;

    public bool WeekInReview { get; set; } = false;

    public bool EventUpdates { get; set; } = false;

    public bool StartupsWeekly { get; set; } = false;

    public bool Podcast { get; set; } = false;
}
