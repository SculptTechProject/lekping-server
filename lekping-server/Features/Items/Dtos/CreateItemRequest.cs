namespace LekPing.Server.Features.Items.Dtos;
using System.ComponentModel.DataAnnotations;

public sealed record CreateItemRequest([Required, StringLength(200)] string Name);
