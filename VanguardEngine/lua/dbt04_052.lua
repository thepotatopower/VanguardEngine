-- 孤影の騎士 フェノーラ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.RC)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Activation")
end

function Activation()
	if not obj.Exists({q.Location, l.AllUnits, q.Other, o.SameColumn, q.Other, o.NotThis}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 10000, p.Continuous)
	end
end