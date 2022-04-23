-- 実験体成功!

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnPut)
	ability1.SetMovedTo(l.Order, l.Player)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.Draw(1)
	obj.Discard(1)
	if obj.GetNumberOf("Filter", {l.Order}) >= 3 and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.ChooseBind({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1})
	end
end

function Filter(id)
	return obj.IsOrderType(id, ot.Research)
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end