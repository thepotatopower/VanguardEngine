-- ディヴァインシスター さんとのーれ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanSendToBottom({q.Location, l.Hand, q.UnitType, u.Normal, q.Count, 1}) end
	obj.SoulBlast(1)
	obj.RevealAndSendToBottom({q.Location, l.Hand, q.UnitType, u.Normal, q.Count, 1})
end

function Activation()
	obj.Draw(1)
end