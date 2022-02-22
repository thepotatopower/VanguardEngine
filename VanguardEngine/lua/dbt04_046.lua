-- 荒躙怪獣 メガグラーゴ

function RegisterAbilities()
	-- on hit
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster()
end

function Condition()
	return obj.CanCounterCharge()
end

function Activation()
	obj.CounterCharge(1)
end