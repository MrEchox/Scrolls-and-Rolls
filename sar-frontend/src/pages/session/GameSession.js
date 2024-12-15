import React from "react";
import PlayerChat from "../../content_pages/chat/PlayerChat";
import StoryChat from "../../content_pages/chat/StoryChat";
import GameSessionItems from "./GameSessionItems";
import { useParams } from "react-router-dom";
import "./GameSession.css";

const GameSession = () => {
  const { sessionId } = useParams();

  return (
    <div className="session">
      <div className="chat-section">
        {sessionId && <PlayerChat sessionId={sessionId} />}
        {sessionId && <StoryChat sessionId={sessionId} />}
      </div>
      {/* Items section 
      <div className="items-section">
        {sessionId && <GameSessionItems sessionId={sessionId} />}
      </div>
      */}
    </div>
  );
};

export default GameSession;
