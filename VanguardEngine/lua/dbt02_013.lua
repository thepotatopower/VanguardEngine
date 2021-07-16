-- Dragritter, Iduriss

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Grade, 2, q.Grade, 3, q.Grade, 4, q.Grade, 5, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.EnemyRetiredThisTurn() and obj.CanCB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	end
end

function Activate(n)
	local selection
	if n == 1 then
		selection = obj.SelectOption("This unit gets Power +10000 until end of turn.", "Put this unit into your soul, choose one of your opponent's grade 2 or greater rear-guards, and retire it.")
		if (selection == 1) then
			obj.AddTempPower(2, 10000)
		else
			obj.AddToSoul(2)
			obj.ChooseRetire(3)
		end
	end
	return 0
end
