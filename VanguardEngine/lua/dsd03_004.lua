-- 天弓の騎士 ベイス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTriggerCondition("Trigger")
	ability1.SetActivationCondition("Condition")
	ability1.SetActivation("OnRide")
end

function Trigger()
	return obj.IsApplicable() 
end

function Condition()
	return obj.Turn() % 2 == 0
end

function OnRide()
	obj.Draw(1)
end