using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerService8.Services
{
    public class ChattingService : ChatService.ChatServiceBase
    {
        private readonly ChatRoom _chatRoomService;

        public ChattingService(ChatRoom chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        public override async Task Join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            do
            {
                _chatRoomService.Join(requestStream.Current.User, responseStream);
                await _chatRoomService.BroadcastMessageAsync(requestStream.Current);
            } while (await requestStream.MoveNext());

            _chatRoomService.Remove(context.Peer);
        }
    }
}
