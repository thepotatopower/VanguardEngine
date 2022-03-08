-- 流麗の美姫 マレイン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsBooster() and obj.EnemyVanguardHit()
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddToHand({q.Other, o.ThisFieldID})
end