-- 咎叩き

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	local count = obj.GetNumberOf({q.Location, l.Bind, q.Other, o.Order})
	obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, count * 5000, p.Continuous)
end