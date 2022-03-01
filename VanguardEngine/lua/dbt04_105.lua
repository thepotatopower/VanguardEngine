-- 天兆の騎士 グランディール

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetLocation(l.Trigger)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.VanguardIs(obj.GetNameFromCardID("dbt04_010")) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusGrade, 2, p.Continuous)
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end