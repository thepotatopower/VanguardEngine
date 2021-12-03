-- 天発の騎士 フルップ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.GC)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Grade, 3}, cs.BonusShield, 5000, p.Continuous)
end