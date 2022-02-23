-- 封焔竜 イーシタ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.IsPlayerTurn() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000 * (1 + obj.GetArmsCount()), p.Continuous)
	end
end
