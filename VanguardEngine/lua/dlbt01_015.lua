-- Fleeting Admiration, Baruel

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.EvenGrade
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 0
	elseif n == 4 then
		return q.Location, l.EnemyRC, q.MaxPower, 0
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.BlackWings, p.SB, 3
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n) 
	if n == 1 then
		if obj.GetNumberOf(2) > 1 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Inject(3, q.Count, math.floor(obj.GetNumberOf(2) / 2))
		obj.ChooseAddTempPower(3, -5000)
		obj.SendToBottom(4)
	end
	return 0
end