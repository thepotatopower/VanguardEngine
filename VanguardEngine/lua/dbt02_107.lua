-- Sublime Will

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Location, l.GC, q.Grade, 3, q.Count, 3
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		if obj.Exists(1) then
			obj.ChooseAddBattleOnlyPower(2, 20000)
		end
	end
	return 0
end