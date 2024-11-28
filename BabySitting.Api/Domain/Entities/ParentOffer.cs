﻿using BabySitting.Api.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabySitting.Api.Domain.Entities;

[Table("ParentOffers")]
public class ParentOffer
{
    [Key]
    public int Id { get; set; }

    public required Guid UserId { get; set; }

    public required string FirstName { get; set; }

    public required string AddressName { get; set; }
    
    public required double AddressLongitude { get; set; }
    
    public required double AddressLatitude { get; set; }

    public required List<LanguagesEnum> FamilySpeakingLanguages { get; set; }

    public required int NumberOfChildren { get; set; }

    public required List<ChildAgeCategoryEnum> ChildrenAgeCategories { get; set; }

    public required List<ChildCharacteristicsEnum> ChildrenCharacteristics { get; set; }

    public required string FamilyDescription {  get; set; }

    public required List<SkillsEnum> PreferebleSkills { get; set; }

    public required CurrencyEnum Currency { get; set; }

    public required int Rate { get; set; } 

    public required JobLocationEnum JobLocation { get; set; }

    public required Schedule Schedule { get; set; } 

}
