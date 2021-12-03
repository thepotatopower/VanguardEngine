-- ディアブロスボーイズ シリル

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Grade, 3, q.Other, o.Attacking})
end

function Activation()
	obj.AddEnemyState(ps.CannotIntercept, p.UntilEndOfBattle)
end