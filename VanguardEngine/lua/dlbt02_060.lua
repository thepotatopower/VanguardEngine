-- 輝ける矜持 イルメリ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanRetire({q.Other, o.This}) end
	obj.SoulBlast(1)
	obj.Retire({q.Other, o.This})
end

function CanFullyResolve()
	return obj.CanSendToBottom("Filter", {l.Drop}, 1)
end

function Filter(id)
	return obj.IsUnitType(id, u.Normal) and not obj.GradeIs(id, 1)
end

function Activation()
	obj.Select("Filter", {l.Drop}, 2, 0, {}, -1)
	obj.SendToBottom({q.Location, l.Selected})
	obj.Shuffle()
end