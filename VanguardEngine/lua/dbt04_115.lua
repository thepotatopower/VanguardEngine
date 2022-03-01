-- 調律狂い

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.Exists({q.Location, l.Drop, q.Location, l.Bind, q.Other, o.Order, q.Count, 5}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end