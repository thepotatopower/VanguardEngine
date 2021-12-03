-- メタライズ・イロジオ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Activation")
end

function Activation()
	if obj.SoulCount() >= 7 then
		local param = {q.Location, l.EnemyRC, q.Other, o.SameColumn}
		obj.AddCardState(param, cs.CannotIntercept, p.Continuous)
		obj.AddCardState(param, cs.CannotGainIntercept, p.Continuous)
		obj.AddCardState(param, cs.CannotBoost, p.Continuous)
		obj.AddCardState(param, cs.CannotGainBoost, p.Continuous)
	end
end