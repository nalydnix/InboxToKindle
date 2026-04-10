import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { supabase } from "../lib/supabase";

export function LoginPage() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [mode, setMode] = useState<"login" | "signup">("login");
  const [status, setStatus] = useState("Use Supabase auth to sign in or create an account.");

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const { error } =
      mode === "login"
        ? await supabase.auth.signInWithPassword({ email, password })
        : await supabase.auth.signUp({ email, password });

    if (error) {
      setStatus(error.message);
      return;
    }

    setStatus(mode === "login" ? "Signed in successfully." : "Check your inbox to confirm your new account.");
    if (mode === "login") {
      navigate("/dashboard");
    }
  }

  return (
    <main className="page-shell auth-shell">
      <section className="hero-panel">
        <p className="eyebrow">InboxToKindle</p>
        <h1>Turn newsletters into Kindle-friendly reading automatically.</h1>
        <p className="muted">
          A clean starting point for signup, subscription email routing, and delivery tracking.
        </p>
      </section>

      <section className="card auth-card">
        <h2>{mode === "login" ? "Sign in" : "Create account"}</h2>
        <form onSubmit={handleSubmit}>
          <label>
            Email
            <input type="email" value={email} onChange={(event) => setEmail(event.target.value)} required />
          </label>
          <label>
            Password
            <input type="password" value={password} onChange={(event) => setPassword(event.target.value)} required />
          </label>
          <button type="submit">Continue</button>
        </form>
        <button className="link-button" type="button" onClick={() => setMode(mode === "login" ? "signup" : "login")}>
          {mode === "login" ? "Need an account? Sign up" : "Already have an account? Sign in"}
        </button>
        <p className="status">{status}</p>
      </section>
    </main>
  );
}
