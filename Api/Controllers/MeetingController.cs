﻿using Application.Common.Models;
using Application.Meetings.Commands.CreateMeeting;
using Application.Meetings.Commands.JoinMeeting;
using Application.Meetings.Commands.RejectInvitation;
using Application.Meetings.Commands.SendInvitation;
using Application.Meetings.Queries.GetMeetingsInvitations;
using Application.Meetings.Queries.MeetingDetails.GetMeetingDetailsById;
using Application.Meetings.Queries.MeetingListItem.GetAllMeetingListItems;
using Application.Meetings.Queries.MeetingPin.GetMeetingPinDetailsById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class MeetingController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingDetailsDto>> GetById(Guid id)
        {
            var meetingDetailsDto = await Mediator.Send(new GetMeetingDetailsByIdQuery() { Id = id });
            return Ok(meetingDetailsDto);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateMeetingCommand command)
        {
            var newMeetingId = await Mediator.Send(command);
            return Created($"/api/Meeting/{newMeetingId}", null);
        }

        [AllowAnonymous]
        [HttpGet("list")]
        public async Task<ActionResult<PagedResult<MeetingListItemDto>>> GetAllMeetingListItems([FromQuery] GetMeetingListItemsQuery request)
        {
            var meetingListItemsDtos = await Mediator.Send(request);
            return Ok(meetingListItemsDtos);
        }

        [AllowAnonymous]
        [HttpGet("pin/{id}")]
        public async Task<ActionResult<MeetingPinDetailsDto>> GetPinDetailsById(Guid id)
        {
            var pinDetailsDto = await Mediator.Send(new GetMeetingPinDetailsByIdQuery() { Id = id });
            return Ok(pinDetailsDto);
        }


        [HttpPost("{meetingId:guid}/join")]
        public async Task<ActionResult> JoinMeeting([FromRoute] Guid meetingId)
        {
            await Mediator.Send(new JoinMeetingCommand() { MeetingId = meetingId });
            return NoContent();
        }
        
        [HttpPost("{meetingId:guid}/reject")]
        public async Task<ActionResult> RejectMeetingInvitation([FromRoute] Guid meetingId)
        {
            await Mediator.Send(new RejectInvitationCommand() { MeetingId = meetingId });
            return NoContent();
        }
        
        [HttpPost("invite")]
        public async Task<ActionResult> SendInvitation([FromBody] SendInvitationCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("invitations")]
        public async Task<ActionResult> GetMeetingsInvitations()
        {
            var meetingInvitationsDtos = await Mediator.Send(new GetMeetingsInvitationsQuery());
            return Ok(meetingInvitationsDtos);
        }
    }
}
