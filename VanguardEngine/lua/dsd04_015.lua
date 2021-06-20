-- Call to the Beasts

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then 
		return q.Location, l.BackRow, q.Count, 3
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, false, false
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.Select(2)
		if obj.Exists(1) then
			obj.AddBattleOnlyPower(3, 15000)
		else
			obj.AddBattleOnlyPower(3, 5000)
		end
	end
	return 0
end