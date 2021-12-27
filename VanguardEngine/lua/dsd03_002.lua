-- 天槍の騎士 ルクス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetCost("OnRideCost")
	ability1.SetActivation("OnRide")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd03_001"))
end

function OnRideCost(check)
	if check then return obj.CanReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 3}) end
	obj.ChooseReveal({q.Location, l.Hand, q.Grade, 3, q.Count, 3})
end

function OnRide()
	obj.Draw(1)
end

function Cont()
	if obj.IsPlayerTurn() and obj.Exists({q.Location, l.PlayerUnits, q.Grade, 3, q.Count, 3}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end