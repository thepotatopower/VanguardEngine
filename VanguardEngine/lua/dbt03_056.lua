-- 豪傑竜 ムサシドアーマー

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetActivation("Activation")
end

function Activation()
	obj.AddCardState({q.Other, o.This}, cs.UniversalPersonaRide, p.Continuous)
end