-- Dragritter, Iduriss

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Grade, 2, q.Other, o.GradeOrHigher, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.EnemyRetiredThisTurn() then
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

function Activate(n)
	local selection
	if n == 1 then
		selection = obj.SelectOption("This unit gets Power +10000 until end of turn.", "Put this unit into your soul, choose one of your opponent's grade 2 or greater rear-guards, and retire it.")
		if (selection == 1) then
			obj.AddTempPower(1, 10000)
		else
			obj.AddToSoul(1)
			obj.ChooseRetire(2)
		end
	end
	return 0
end
