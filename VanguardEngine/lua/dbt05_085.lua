-- 怨讐の銛兵

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.RC)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsPlayerTurn() then
		local power = obj.GetNumberOf({q.Location, l.Bind, q.Other, o.Order}) * 5000
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, power, p.Continuous)
	end
end