-- 変光星の継子

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsPlayerTurn() then
		local power = obj.GetNumberOf({q.Location, l.Order, q.Other, o.SetOrder}) * 2000
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, power, p.Continuous)
	end
end