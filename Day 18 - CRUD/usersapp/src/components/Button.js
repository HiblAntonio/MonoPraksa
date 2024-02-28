import React from "react";

export default function Button({ text, onClick, href }) {
    if (href) {
      return (
        <a href={href} className="button" onClick={onClick}>
          {text}
        </a>
      );
    } else {
      return (
        <button className="button" onClick={onClick}>
          {text}
        </button>
      );
    }
  };
